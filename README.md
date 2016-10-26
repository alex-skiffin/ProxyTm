# ProxyTm
## Прокси-сервер

Прокси-сервер запускается локально. Значения по умолчанию:
- Порт - 7070
- Сайт - https://habrhabr.ru
- Размер помечаемого слова - 6 букв

Сервер можно запустить со своими параметрами, передав их через командную строку, либо изменив значения в файле конфигурации.

> proxytm.exe 12332 https://geektimes.ru 5

Первым параметром должен идти порт (например 12332).
Второй параметр - адрес сайт без указания http или https. по умолчанию сайт ищется по https.
Третий параметр - размер слов, которые будут помечаться значком "™".

Пример стандартной конфигурации:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <appSettings>
        <add key="wordSize" value="6" />
        <add key="port" value="7070" />
        <add key="url" value="https://habrahabr.ru" />
    </appSettings>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
</configuration>
```