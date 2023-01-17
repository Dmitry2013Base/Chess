# Chess
Для запуска выполняем:
```
cd Chess
dotnet run
```
Теперь по адресу https://localhost:7148 попадаем на главную страницу

Запуск с помощью docker:
```
cd Chess
docker build . -t chess:latest
docker run -it -p 5000:80 chess
```
Теперь по адресу http://localhost:5000 попадаем на главную страницу
