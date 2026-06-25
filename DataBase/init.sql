CREATE DATABASE IF NOT EXISTS companycrm;
USE companycrm;
    
CREATE TABLE IF NOT EXISTS employees(
    id INT PRIMARY KEY AUTO_INCREMENT,
    full_name VARCHAR(100) NOT NULL,
    position INT NOT NULL COMMENT '0 - Руководитель, 1 - Работник',
    birth_date DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS contractors(
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(200) NOT NULL,
    inn VARCHAR(12) NOT NULL,
    curator_id INT NOT NULL,
    FOREIGN KEY (curator_id) REFERENCES employees(id)
    );

CREATE TABLE IF NOT EXISTS orders(
    id INT PRIMARY KEY AUTO_INCREMENT,
    date DATETIME NOT NULL,
    amount DECIMAL(18, 2) NOT NULL,
    employee_id INT NOT NULL,
    contractor_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employees(id),
    FOREIGN KEY (contractor_id) REFERENCES contractors(id)
    );

INSERT INTO employees (full_name, position, birth_date) VALUES
('Иванов Иван Иванович', 0, '1985-03-15 00:00:00'),
('Петрова Анна Сергеевна', 1, '1990-07-22 00:00:00'),
('Сидоров Алексей Петрович', 1, '1988-11-01 00:00:00'),
('Козлова Елена Викторовна', 1, '1995-02-10 00:00:00');

INSERT INTO contractors (name, inn, curator_id) VALUES
('ООО "Ромашка"', '123456789012', 1),
('ИП "Весна"', '987654321098', 2),
('ЗАО "Техносфера"', '567890123456', 1),
('ООО "Альфа-Групп"', '432109876543', 3);

INSERT INTO orders (date, amount, employee_id, contractor_id) VALUES
('2026-01-15 10:30:00', 15000.50, 1, 1),
('2026-01-20 14:15:00', 23000.00, 2, 2),
('2026-02-01 09:00:00', 7800.75, 3, 3),
('2026-02-10 16:45:00', 42000.00, 1, 4),
('2026-02-15 11:20:00', 12500.00, 4, 2);