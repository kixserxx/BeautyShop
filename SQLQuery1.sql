insert into user_role (role_name)
values (N'Администратор'),
	   (N'Парикмахер'),
	   (N'Клиент');
insert into users (fio, role_id, user_phone, user_email, user_password)
values (N'Воробьева Инна Михайловна', 1, '+7 (000) 000-00-00','inna@yandex.ru', '123'),
	   (N'Иванова Марина Алексеевна', 2, '+7 (111) 111-11-11','ivanova201@mail.ru', '123'),
       (N'Романова Ангелина Олеговна', 3, '+7 (222) 222-22-22', 'romolga@bk.ru', '123');
insert into sale (sale_name, sale_description, sale_price)
values (N'Абсолютное счастье для волос', 'Самая популярная процедура для восставления волос. В результате процедуры вы станете обладательницей бесконечно красивых и здоровых волос!', 2990);
insert into service_type (type_name)
values (N'Для волос');
insert into service_work (type_id, service_name, service_price)
values (1, N'Окрашивание волос', 15000);
insert into record (client_id, master_id, date_record, service_id, sale_id, record_price) 
values (3, 2, '2024-02-13 16:45:00', 1, null, 15000);