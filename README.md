# CSharp-MVC

ASP .NET сайт "Файловое хранилище" для загрузки и скачивания файлов.
Описание задания:
■	Страница «Загрузка файла»: элемент выбора файла с компьютера и кнопка «Загрузить». Файл загружается и кладется в папку на жестком диске, для файла генерируется запись в БД. Id загруженного файла отображается пользователю;
■	Страница «Скачивание файла»: поле для ввода Id файла и кнопка «Скачать» по нажатию, проверяется наличие данных о файле в БД, содержимое файла скачивается в браузере;
■	Данные о файлах хранятся в базе данных, в таблице Files. Папка для хранения и максимальный размер файла задаются в конфигурационном файле (web.config) в секции appSettings.