# Azure Tablas Fáciles

Durante el módulo anterior escribimos una aplicación Xamarin.Forms que tomaba información expuesta en un servicio RESTful. En este módulo vamos a configurar Azure para poder usar Mobile Apps. 

## Creación de Azure Mobile Apps usando Tablas Fáciles

Entrar a la siguiente url: http://portal.azure.com 

1. Una vez dentro del portal hacer click en **Crear nuevo recurso**, escribir en el buscador **mobile apps** para que salgan los resultados como en la imagen de abajo. 

![Buscar Mobile App](./images/01.BuscarMobileApp.PNG)

2. Luego, seleccionar **Mobile apps Quickstart**.

![Seleccionar Mobile App](./images/02.SeleccionarMobileApp.PNG)

3. Finalmente, seleccionar **Crear** para que Azure cree el nuevo recurso.

![Mobile App QuickStart](./images/03.MobileAppQuickstart.PNG)

En el caso de que el usuario que estemos utilizando no tenga una suscripcion a Azure, nos va a pedir que obtengamos una.

![Suscripcion](./images/04.Suscripcion.PNG)

4. Se va a abrir una ventana con cuatro campos para configurar:

    **Nombre de la aplicación:**
    Es un nombre único que va a ser necesario para configurar el end point. 

    **Suscripción:**
    Seleccionamos una suscripción de la lista.

    **Grupo de recursos:**
    Seleccionamos **Crear uno nuevo** y escribimos el mismo nombre que la aplicación.

    **Plan de App Service/Ubicación:**
    Hacemos click en este campo y seleccionamos **Crear nuevo**, le damos un nombre único y seleccionamos el plan F1 Free.

Por último, clickear en **Anclar al panel** y luego en **Crear**. Va a tardar entre unos 3 y 5 minutos en crear el nuevo recurso. 

![Creacion](./images/05.Creacion.PNG)

5. Si volvemos al Dashboard vamos a poder ver tanto el nuevo recurso como nuestra suscripción a Azure.

![Dashboard](./images/06.Dashboard.PNG)

6. Seleccionar nuestro recurso y buscar en el menu lateral izquierdo **Tablas fáciles**

![Dashboard](./images/07.OpcionTablasFaciles.PNG)

7. Haciendo click en **Agregar desde CSV** vamos a poder agregar una nueva tabla con un conjunto de datos. Hay que asegurarse de haber bajado de este repositorio los archivos speaker.csv y sessions.csv que se encuentran en la carpeta Mock-Data

![Agregar CSV](./images/08.AgregarCSV.PNG)

## Testeando el endpoint

Ahora que ya hemos subido nuestros datos exitosamente a nuestro backend de Azure, podemos probar que esté funcionando correctamente. Solo tenemos que copiar la url del Mobile Appsd Service que aparece en **Introducción**, dicha opcion se encuentra en el menu lateral izquierdo.

![Test CSV](./images/09.TestRecurso.PNG)

Abrir un browser y pegarle a los recursos con el siguiente formato:

**Speaker**
```
https://<yourappservicename>.azurewebsites.net/tables/Speaker?ZUMO-API-VERSION=2.0.0 
```
**Session**
```
https://<yourappservicename>.azurewebsites.net/tables/Session?ZUMO-API-VERSION=2.0.0 
```
Ambos deberian devolverte la información en formato `json`.

Ahora, vamos a editar el código para acceder a la información de la aplicación XamarinAssemble.

## Conectándonos desde la App al backend de Azure Mobile Apps

Para conectarnos al backend de Azure Mobile apps y poder aprovechar todas las ventajas del soporte offline que nos provee, necesitamos instalar dos paquetes en nuestros proyectos: `Microsoft.Azure.Mobile.Client`y `Microsoft.Azure.Mobile.Client.SQLiteStore`. Para agilizar, ya estan instalados, pero de todas formas podes verificarlo clickeando con el botón derecho del mouse sobre la solución y seleccionando la opción **Manage Nuget Packages for solution...**

![Azure Nuget Packages](https://raw.githubusercontent.com/nishanil/Dev-Days-HOL/master/02%20Cloud-Labs/screenshots/Azure-Nuget-Packages.png?token=AC9rtmcdDk5_XxPfM26Vts8NNRBto9O0ks5X0sP5wA%3D%3D)

Vamos a abrir el archivo **XamarinAssemble/Constants.cs** y a actualizar la variable `ApplicationURL` para que apunte a la URL del servicio Mobile App en el formato especificado. 

```csharp
public static string ApplicationURL = @"https://<yourappservicename>.azurewebsites.net";
```

### Conectándonos a la nube

El código para conectarnos a la nube esta en la clase `Cloud/AzureDataManager.cs`. Los SDK de Azure Client tienen un gran soporte para la sincronización de datos offline. Lo que significa que cuando la aplicación esta en modo offline, los usuarios pueden crear y modificar los datos, los cuales son guardados en una base de datos local. Cuando la aplicación vuelve a estar online, se sincronizan las modificaciones realizaon el backend de Azure Mobile App. Esta característica tambien incluye la detección de conflictos cuando el mismo registro ha sido modificado tanto en el cliente como en el backend. Los conflictos pueden ser solucionados tanto en el servidor como en el cliente. Para aprovechar estas ventajas, esta demo usa `SyncTable` e inicializa una base de datos local. 

De la siguiente manera, inicializamos todo lo necesario:

```csharp
private void Initialize()
{
    this.client = new MobileServiceClient(
        Constants.ApplicationURL);

    var store = new MobileServiceSQLiteStore("localstore.db");
    store.DefineTable<Session>();
    store.DefineTable<Speaker>();

    //Initializes the SyncContext using the default IMobileServiceSyncHandler.
    this.client.SyncContext.InitializeAsync(store);
}
```

Para obtener la información de `Sessions` y `Speakers`, el `AzureDataManager` tiene dos métodos `GetSessionsAsync()` y `GetSpeakersAsync()` los cuales pueden ser accedidos por la implementación de `IDataManager`.

### Get Sessions

Abrir el archivo **XamarinAssemble\ViewModels\SessionsViewModel.cs**, en el método `GetSessions()` reemplazar el código

```csharp
using (var client = new HttpClient())
{
    //grab json from server
    var json = await client.GetStringAsync("https://xamarinassemblebaires.azurewebsites.net/tables/sessions?ZUMO-API-VERSION=2.0.0");

    //Deserialize json
    var items = JsonConvert.DeserializeObject<List<Session>>(json);

    //Load sessions into list
    Sessions.Clear();

    foreach (var item in items)
    {
        Sessions.Add(item);
    }
}
```
con el código para conectarnos a la nube:

```csharp
var items = await AzureDataManager.DefaultManager.GetSessionsAsync();

//Load sessions into list
Sessions.Clear();

foreach (var item in items)
{
    Sessions.Add(item);
}
```

### Get Speakers

Abrir el archivo **XamarinAssemble\ViewModels\SpeakersViewModel.cs**, en el método `GetSpeakers()` reemplazar el código

```csharp
using (var client = new HttpClient())
{
    //grab json from server
    var json = await client.GetStringAsync("https://xamarinassemblebaires.azurewebsites.net/tables/speakers?ZUMO-API-VERSION=2.0.0");

    var items = JsonConvert.DeserializeObject<List<Speaker>>(json);

    Speakers.Clear();
    foreach (var item in items)
    {
        Speakers.Add(item);
    }
}
```
con el código para conectarnos a la nube

```csharp
var items = await AzureDataManager.DefaultManager.GetSpeakersAsync();

Speakers.Clear();

foreach (var item in items)
{
    Speakers.Add(item);
}
```

Y eso es todo! Ahora podemos correr la aplicación y ver los datos que son traídos desde el backend de Azure Tablas Faciles.

## Correr la aplicación!
Corramos la aplicación en todas las plataformas disponibles. Entremos al tab de Speakers y hagamos click en cualquier speaker, editemos el título y guardemos los cambios.Veamos si la información cambia. Ademas, peguemosle a la url de vuelta para ver si la información se actualizó en el backend.


