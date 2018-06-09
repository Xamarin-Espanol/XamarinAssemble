# Construyendo una aplicacion en Xmarin Forms

Durante todo el workshop vamos a trabajar con un proyecto desde 0, generando una aplicación en la cual se pueda traer informacion sobre los speakers y las charlas que se estuvieron dando.

En este modulo vamos a aprender a generar una aplicación y como ejecutarla, nos servira además para saber si tenemos el ambiente correctamente instalado para poder avanzar con el workshop.

En la carpeta **Start** vamos a encontrar una Solucion ya empezada, a la cual le vamos a realizar ciertas modificaciones. Asi que vamos a abrir esa Solucion.

## Creando Model y ViewModels

## Model
La informacion de los speakers va a ser traida de un endpoint del tipo rest. La clase **Speaker** es el modelo que es usado para guardar dichos datos. 

1. Abrir el archivo **MisEventos/Models/Speaker.cs** y agregar las siguientes propiedades dentro de la clase **Speaker**.

```csharp
[JsonProperty("name")]
public string Name { get; set; }

[JsonProperty("description")]
public string Description { get; set; }

[JsonProperty("image")]
public string Image { get; set; }

[JsonProperty("title")]
public string Title { get; set; }

[JsonProperty("company")]
public string Company { get; set; }

[JsonProperty("website")]
public string Website { get; set; }

[JsonProperty("blog")]
public string Blog { get; set; }

[JsonProperty("twitter")]
public string Twitter { get; set; }

[JsonProperty("email")]
public string Email { get; set; }

[JsonProperty("avatar")]
public string Avatar { get; set; }

[JsonProperty("webiste")]
public string Webiste { get; set; }

[JsonProperty("titile")]
public string Titile { get; set; }

[JsonProperty("biography")]
public string Biography { get; set; }
```

## ViewModel
El archivo **SpeakersViewModel.cs** nos va a proveer de toda la funcionalidad para poder mostrar la informacion. Va a consistir de una lista de speakers y un metodo que puede ser llamado para obtener los speakers desde el servidor. Tambien, va a contener un flag booleano que va a indicar si ya estamos obteniendo la informacion de los speakers en una tarea en segundo plano. 

1. Abrir el archivo **MisEventos/ViewModels/SpeakersViewModel.cs** para trabajar sobre el.

Vamos a utilizar un **ObservableCollection** para guardar la informacion de los speakers dentro del ViewModel. Utilizaremos **ObservableCollection** porque soporta al evento **CollectionChanged**, el cual es un evento al que la vista se suscribe y automaticamente se actualiza cuando la informacion es añadida o removida.

2. Copiar el siguiente codigo arriba del constructor de **SpeakerViewModel** para poder declarar una property

```csharp
public ObservableCollection<Speaker> Speakers { get; set; }
```

3. Dentro del constructor de la clase, crear una nueva instancia de **ObservableCollection**:

```csharp
public SpeakersViewModel()
{
    Speakers = new ObservableCollection<Speaker>();
    Title = "Speakers";
}
```

### Metodo GetSpeakers
1. Crear un metodo llamado **GetSpeakers** el cual va a devolver la lista de speakers desde el endpoint que mencionamos anteriormente. Primero, vamos a implementar un simple HTTP request, despues vamos a ir actualizando este metodo hasta que podamos tomar la informacion de Azure.

El metodo que estamos creando va a ser del tipo async, dado que es una Task.

```csharp
private async Task GetSpeakers()
{

}
```

2. Vamos a agregar el siguiente codigo dentro del metodo que acabamos de crear, para asegurarnos que todavia estamos tomando informacion del endpoint.

```csharp
if(IsBusy)
        return;
```

3. Lo siguiente que vammos a hacer es agregar el bloque * *try/catch* *:

```csharp
private async Task GetSpeakers()
{
    if (IsBusy)
        return;

    Exception error = null;
    try
    {
        IsBusy = true;

    }
    catch (Exception ex)
    {
        error = ex;
    }
    finally
    {
       IsBusy = false;
    }

}
```

El atributo * *IsBusy* * esta en true cuando realizamos la llamada al servidor, y en false cuando finaliza esa llamada.

4. Usaremos * *HttpClient* * para tomar el json desde el servidor dentro del bloque try:

```csharp
using(var client = new HttpClient())
{
   //grab json from server
   var json = await client.GetStringAsync("https://demo4404797.mockable.io/speakers");
}
```

5. Dentro del **using** vamos a deserializar el json que es enviado por el servidor y a convertirlo en una lista de Speakers usando Json.NET:

```csharp
var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
```

6. Agregar dentro el **using** el codigo para poder limpiar la lista de speakers y luego ir añadiendolos dentro del ObservableCollection:

```csharp
Speakers.Clear();
foreach (var item in items)
    Speakers.Add(item);
```

7. Si algo llega a salir mal dentro del try, el **catch** va a tomar la excepcion y despues del bloque finally vamos a mostrar una alerta.

```csharp
if (error != null)
    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
```

El codigo completo despues de todas las lineas que fuimos agregando deberia verse asi:

```csharp
private async Task GetSpeakers()
{
    if (IsBusy)
        return;

    Exception error = null;
    try
    {
        IsBusy = true;
        
        using(var client = new HttpClient())
        {
            //grab json from server
            var json = await client.GetStringAsync("https://demo4404797.mockable.io/speakers");
            
            //Deserialize json
            var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
            
            //Load speakers into list
            Speakers.Clear();
            foreach (var item in items)
                Speakers.Add(item);
        } 
    }
    catch (Exception ex)
    {
        Debug.WriteLine("Error: " + ex);
        error = ex;
    }
    finally
    {
        IsBusy = false;
    }

    if (error != null)
        await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
}
```

Y asi acabamos de terminar nuestro metodo para obtener toda la informacion acerca de los speakers!


### GetSpeakers Command
En vez de invocar al metodo directamente, lo vamos a exponer con un **Command**. Un Command es una interface que conoce que metodo invocar y tiene un camino opcional para saber si el Command no esta ocupado.

1. Creamos un nuevo Command llamado **GetSpeakersCommand**:

```csharp
public Command GetSpeakersCommand { get; set; }
```

2. Dentro del constructor del **SpeakersViewModel** creamos el **GetSpeakersCommand** y le pasamos dos metodos, uno para invocar al comando y otro para saber cuando el comando esta libre. 

```csharp
GetSpeakersCommand = new Command(
                async () => await GetSpeakers(),
                () => !IsBusy);
```

## Creando UI en XAML y DataBinding
Ahora construiremos nuestra interface en el archivo **Views/SpeakersPage.xaml**

### SpeakersPage.xaml
En esta pagina vamos a agregar BLA BLA BLA BLA 

### Boton de sincronizacion

