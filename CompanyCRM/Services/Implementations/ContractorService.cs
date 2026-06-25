using System.Collections.Generic;
using System.Linq;
using CompanyCRM.Common;
using CompanyCRM.Data.Repositories;
using CompanyCRM.MVVM.Models;

namespace CompanyCRM.Services
{
    public sealed class ContractorService : IContractorService
    {
        private readonly IContractorRepository _contractorRepository;
        private readonly IOrderRepository _orderRepository;

        public ContractorService(
            IContractorRepository contractorRepository,
            IOrderRepository orderRepository)
        {
            _contractorRepository = contractorRepository;
            _orderRepository = orderRepository;
        }

        public Contractor GetById(int id)
        {
            return _contractorRepository.GetById(id);
        }

        public IList<Contractor> GetAll()
        {
            return _contractorRepository.GetAll();
        }

        public Result TrySave(Contractor contractor)
        {
            if (contractor == null)
                return Result.CreateFailed("Контрагент не может быть null");

            if (string.IsNullOrWhiteSpace(contractor.Name))
                return Result.CreateFailed("Наименование обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(contractor.Inn))
                return Result.CreateFailed("ИНН обязателен для заполнения");

            if (contractor.Inn.Length != 12)
                return Result.CreateFailed("ИНН должен содержать 12 цифр");

            if (contractor.Curator == null)
                return Result.CreateFailed("Не выбран куратор");

            _contractorRepository.Save(contractor);
            return Result.CreateSuccessed();
        }

        public Result TryDelete(int id)
        {
            var contractor = _contractorRepository.GetById(id);
            
            if (contractor == null)
                return Result.CreateFailed("Контрагент не найден");

            var hasOrders = _orderRepository.GetAll().Any(o => o.Contractor.Id == id);
            if (hasOrders)
                return Result.CreateFailed("Нельзя удалить контрагента, так как у него есть заказы");
            
            _contractorRepository.Delete(id);
            return Result.CreateSuccessed();
        }
    }
}