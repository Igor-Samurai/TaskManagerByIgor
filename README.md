Приложением можно воспользоваться, запустив TestTaskManager.exe, располагающегося по пути TestTaskManager\TestTaskManager\bin\Debug\net10.0-windows\TestTaskManager.exe

В корне репозитория лежит файл UserTasks.JSON представляющий базу данных.

В файле appsettings.json можно прописать путь к папке, в которой лежит файл UserTasks.JSON. Файл appsettings.json расположен по пути TestTaskManager\TestTaskManager\bin\Debug\net10.0-windows\TestTaskManager.exe

Если Вы не укажете путь к файлу UserTasks.JSON или укажете его неправильно, то программа автоматически создаст пустой файл рядом с TestTaskManager.exe (в папке TestTaskManager\TestTaskManager\bin\Debug\net10.0-windows).

Если файл UserTasks.JSON будет поврежден и его не удастся считать, то он перезапищется в правильном формате, новыми записями, которые Вы добавите.
