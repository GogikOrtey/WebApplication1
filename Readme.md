﻿## Проект по созданию своего API

### Задание: 

Сделать API. Надо, чтобы при запросу к нему выводилась вся структура папок и подпапок (файлы не надо) с диска С

### Реализация: 

Главный файл с реализацией сканирования: 📘 ScanDiscC.cs 📘  
Файл, где описаны методы: 📗 Controllers/StructDiskCController.cs 📗

Скриншот выполнения главного запроса в Swagger:

![](01.png)

---

Дополнительно к коду сканирования структуры диска С я добавил:

Счётчик строк, количества уровней

Вывод текущей даты и времени, а также количества времени, затраченного на сканирование

Сделал красивую структуру вывода в консоли в виде дерева. Можно выводить с линиями, а можно просто с пробелами. Один уровень вложенности = 2 пробела.

Программа работает долго. Хотел использовать параллельные вычисления. С ними код начал работать в 3 раза быстрее, но неправильно. Так что пришлось делать без них

Тогда мне пришла идея кешировать результаты выполнения сканирования, и при запросе через API выдавать их. Потому что сканирование всего диска С занимает примерно 2 минуты, а кэшированные результаты будут выдаваться мгновенно.

И я создал 2 метода GET: Один моментально возвращает кэшированные результаты сканирования диска С, если они есть и считаются валидными (сканирование было произведено на этом компьютере, и было меньше суток назад). В противном случае запускается новое сканирование

И 2й метод - при вызове, запускает новое полное сканирование структуры диска С, которое занимает от 2 до 5 минут.

В выводе и описании и выводе я добавил красные и зелёные эмодзи квадратики, что бы было более наглядно. В JSON они отображаются корректно, но если проект будет использоваться только в консоли, то их конечно можно убрать.
