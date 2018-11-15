# Construyendo una aplicación en Xamarin Forms

Durante el workshop, vamos a trabajar con un proyecto desde 0, generando una aplicación en la cual se pueda traer información sobre los speakers y las charlas que se dieron.

Además de generar la aplicación y ejecutarla, nos servirá para saber si tenemos el ambiente correctamente instalado para poder avanzar con el workshop.

En la carpeta **Start** vamos a encontrar una Solución ya empezada, a la cual le vamos a realizar ciertas modificaciones. Abrimos la Solución.

## Creando Model y ViewModels

### Model
La información de los speakers va a ser traída de un endpoint del tipo rest. La clase **Speaker** es el modelo usado para guardar dichos datos. 

1. Abrir el archivo **XamarinAssemble/Models/Speaker.cs** y agregar las siguientes propiedades dentro de la clase **Speaker**.

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

### ViewModel
El archivo **SpeakersViewModel.cs** nos va a proveer de toda la funcionalidad para poder mostrar la información. Va a consistir en una lista de speakers y un mêtodo que puede ser llamado para obtener los speakers desde el servidor. También va a contener un flag booleano que va a indicar si ya estamos obteniendo la información de los speakers en una tarea en segundo plano. 

1. Abrir el archivo **XamarinAssemble/ViewModels/SpeakersViewModel.cs** para trabajar sobre él.

Vamos a utilizar un **ObservableCollection** para guardar la información de los speakers dentro del ViewModel. Utilizaremos **ObservableCollection** porque soporta al evento **CollectionChanged**, el cual es un evento al que la vista se suscribe y automaticamente se actualiza cuando la información es añadida o removida.

2. Copiar el siguiente código arriba del constructor de **SpeakerViewModel** para poder declarar una property:

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

### Método GetSpeakers
1. Crear un método llamado **GetSpeakers** en el archivo **XamarinAssemble/ViewModels/SpeakersViewModel.cs**, el cual va a devolver la lista de speakers desde el endpoint que mencionamos anteriormente. Primero, vamos a implementar un simple HTTP request. Después vamos a ir actualizando este método hasta que podamos tomar la información de Azure.

El método que estamos creando va a ser del tipo async, dado que es una Task.

```csharp
private async Task GetSpeakers()
{

}
```

2. Vamos a agregar el siguiente código dentro del método que acabamos de crear, para asegurarnos que todavía estamos tomando información del endpoint. Si * *IsBusy* * se encuentra en true retornamos y no llamamos al endpoint.

```csharp
if(IsBusy)
        return;
```

3. Lo siguiente que vamos a hacer es agregar el bloque * *try/catch* *:

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
   var json = await client.GetStringAsync("https://xamarinassemblebaires.azurewebsites.net/tables/speakers?ZUMO-API-VERSION=2.0.0");
}
```

5. Dentro del **using** vamos a deserializar el json que es enviado por el servidor y a convertirlo en una lista de Speakers usando Json.NET:

```csharp
var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
```

6. Agregar dentro el **using** el código para poder limpiar la lista de speakers y luego ir añadiendolos dentro del ObservableCollection:

```csharp
Speakers.Clear();
foreach (var item in items)
{
    Speakers.Add(item);
} 
```

7. Si algo llega a salir mal dentro del try, el **catch** va a tomar la excepción y después del bloque finally, vamos a mostrar una alerta.

```csharp
if (error != null)
    await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
```

El código completo despues de todas las lineas que fuimos agregando deberia verse asi:

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
            var json = await client.GetStringAsync("https://xamarinassemblebaires.azurewebsites.net/tables/speakers?ZUMO-API-VERSION=2.0.0");
            
            //Deserialize json
            var items = JsonConvert.DeserializeObject<List<Speaker>>(json);
            
            //Load speakers into list
            Speakers.Clear();
            foreach (var item in items)
            {
                Speakers.Add(item);
            } 

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

Y asi terminamos nuestro método para obtener toda la información acerca de los speakers!


### GetSpeakers Command
En vez de invocar al método directamente, lo vamos a exponer con un **Command**. Un Command es una interface que conoce que método invocar y tiene un camino opcional para saber si el Command no esta ocupado.

1. Creamos un nuevo Command llamado **GetSpeakersCommand** en el ViewModel de los speakers:

```csharp
public Command GetSpeakersCommand { get; set; }
```

2. Dentro del constructor del **SpeakersViewModel** creamos el **GetSpeakersCommand** y le pasamos dos métodos, uno para invocar al command y otro para saber cuando el command esta libre. 

```csharp
 GetSpeakersCommand = new Command(async () => await GetSpeakers());
```

## Creando UI en XAML y DataBinding
Ahora construiremos nuestra interface en el archivo **Views/SpeakersPage.xaml**

### SpeakersPage.xaml
Vamos a agregar un **AbsoluteLayout** a la página y dentro de ese layout, agregamos los controles.

1. Entre los tags de inicio y de cierre del ContentPage agregar el siguiente código:
```csharp
   <AbsoluteLayout HorizontalOptions="FillAndExpand" VerticalOptions="FillAndExpand">

    </AbsoluteLayout>
```

2. Agregar un **StackLayout** dentro del **AbsoluteLayout**

```csharp
<StackLayout
    AbsoluteLayout.LayoutFlags="All"
    AbsoluteLayout.LayoutBounds="0,0,1,1">
</StackLayout>
```
3. Usaremos un **ListView** que va a hacer el binding entre la colección de speakers para poder mostrar todos los items.  Podemos usar una propiedad especial llamada x:Name="" para nombrar cada control de la forma que queramos. 

```csharp
<StackLayout
    AbsoluteLayout.LayoutFlags="All"
    AbsoluteLayout.LayoutBounds="0,0,1,1">
        <ListView x:Name="ListViewSpeakers"
            ItemsSource="{Binding Speakers}">
               <!--Add ItemTemplate Here-->
        </ListView>
</StackLayout>
```
4. Necesitamos describir como se van a ver cada uno de los items. Para hacer esto, podemos usar un **ItemTemplate** que contenga un **DataTemplate**. Xamarin.Forms tiene por default Cells que podemos usar. En este caso vamos a utilizar **ImageCell**, que contiene una imagen y dos filas de texto.

Agregar lo siguiente dentro del ListView:

```csharp
<ListView.ItemTemplate>
    <DataTemplate>
        <ImageCell Text="{Binding Name}"
                    Detail="{Binding Title}"
                    ImageSource="{Binding Avatar}"/>
    </DataTemplate>
</ListView.ItemTemplate>
```

5. Bajo el ListView podemos mostrar una barra de loading cuando se esten obteniendo datos del servidor. Podemos usar un **ActivityIndicator** para hacer eto y bindearlo con la propiedad * *IsBusy* * que nombramos anteriormente.
Para hacer todo esto, vamos a añadir un nuevo **StackLayout** debajo del que teniamos y adentro escribimos el código para mostrar el **ActivityIndicator**

```csharp
<StackLayout IsVisible="{Binding IsBusy}"
             Padding="32"
             AbsoluteLayout.LayoutFlags="PositionProportional"
             AbsoluteLayout.LayoutBounds="0.5,0.5,-1,-1">
    <ActivityIndicator IsRunning="{Binding IsBusy}"/>
</StackLayout>
```

### Botón de sincronización
1. Agregar un **ToolBarButton** al XAML y bindearlo al Command **GetSpeakersCommand**. Esto hará que se ejecute la llamada al servidor cada vez que se haga click sobre el botón. Escribir el código que se encuentra debajo entre los tags del **ContentPage**

```csharp
<ContentPage.ToolbarItems>
    <ToolbarItem Text="Sync" Command="{Binding GetSpeakersCommand}" />
</ContentPage.ToolbarItems>
```
 
### DataBinding
Finalmente, para poder realizar el Binding entre el ViewModel y la vista, en el archivo **XamarinAssemble/Views/SpeakersPage.xaml.cs** reemplazar:
```csharp
public SpeakersPage()
{
    InitializeComponent();
}

protected override void OnAppearing()
{
    base.OnAppearing();
}
```
por
```csharp
private SpeakersViewModel speakersViewModel;

public SpeakersPage()
{
    InitializeComponent();

    speakersViewModel = new SpeakersViewModel();
    BindingContext = speakersViewModel;
}

protected override async void OnAppearing()
{
    base.OnAppearing();

    await speakersViewModel.Initialization;
}
```

### Insertar la página de los speakers a la MainPage
Como nuestra **MainPage** se compone de tabs, por ser una **TabbedPage**, tenemos que agregar la **SpeakersPage** como un hijo de esta. 

1. Abrir el archivo **App.Xaml.cs** y agregarle al constructor el siguiente código:

```csharp
var speakersPage = new NavigationPage(new SpeakersPage()) { Title = "Speakers" };
```

2. Agregar la nueva **NavigationPage** speakersPage a la **MainPage** de la siguiente forma:

```csharp
mainPage.Children.Add(speakersPage);
```

Nuestro código deberia ser similar al siguiente:

```csharp
public App()
{
    InitializeComponent();

    var mainPage = new TabbedPage();
    var sessionsPage = new NavigationPage(new SessionsPage()) { Title = "Sessions" };
    var speakersPage = new NavigationPage(new SpeakersPage()) { Title = "Speakers" };
    var aboutPage = new NavigationPage(new AboutPage()) { Title = "About" };

    mainPage.Children.Add(sessionsPage);
    mainPage.Children.Add(speakersPage);
    mainPage.Children.Add(aboutPage);

    if (Device.RuntimePlatform == Device.iOS)
    {
        sessionsPage.Icon = "tab_feed.png";
        speakersPage.Icon = "tab_person.png";
        aboutPage.Icon = "tab_about.png";
    }

    MainPage = mainPage; 
}
```

### Page Navigations
Xamarin.Forms provee varias experiencias de navegación, dependiendo del tipo de página que estemos usando. Nosotros usamos **TabbedPage** en nuestro **MainPage** para poder obtener una navegación más sencilla. Ahora vamos a desarrollar una página de maestro-detalle. Cuando los usuarios seleccionen un item de la lista de Sessions, vamos a navegar hasta la página del detalle. Usaremos Hierarchical Navigation en la clase NavigationPage, la cual provee la experiencia de navegabilidad class en la cual el usuario puede navegar a través de las distintas páginas, ya sea para adelante o para atrás, según desee. 

1. Agregamos el siguiente código en el archivo **SessionsPage.xaml.cs**:

```csharp
async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
{
    var item = args.SelectedItem as Session;
    if (item == null)
        return;

    await Navigation.PushAsync(new SessionDetailPage() { BindingContext = new SessionDetailViewModel(item)});
    
    // Manually deselect item
    SessionsListView.SelectedItem = null;
}
```
Es necesario invocar al modo PushAsync, ya que esto causa que la instancia de SessionDetailPage sea apilada en el stack de navegación, donde se convierte en una página activa. Esta puede ser desapilada del stack de navegación presionando el botón Atrás del dispositivo. Otra alternativa es llamar al método PopAsync en el código.

### Ejecuta la aplicación!

Ejecuta la aplicación en las distintas plataformas para notar las diferencias. 

## Platform Customizations

Hasta ahora escribimos cada línea de código en nuestro proyecto común (XamarinAssemble), que nos permite compartir el 100% de código para todas las plataformas. Xamarin.Forms es extensible y te deja incorporar features específicas para cada plataforma. Podes utilizar la clase `Device` para crear comportamiento específico dentro del código compartido y en la interfaz de usuario (incluido en XAML) para customizaciones sencillas. Si tenes customizaciones complejas, se puede utilizar `DependencyService` para invocar el código nativo desde la capa compartida. Los `CustomRenderers`  pueden ser utilizados para pequeños cambios de estilos o para sofisticados layout y comportamientos que utilicen características específicas de cada plataforma.

### Device.RuntimePlatform

En iOS, los Tabs pueden mostrar iconos junto con el título. En tu aplicación, como el requerimiento es específico para iOS, vamos a verificar que la plataforma sea iOS con la propiedad `Device.RuntimePlatform` para asignar los iconos.

Agrega este código dentro del constructos del archivo **App.xaml.cs**

```chsarp
Device.OnPlatform(iOS: () => {
    sessionsPage.Icon = "tab_feed.png";
    speakersPage.Icon = "tab_person.png";
    aboutPage.Icon = "tab_about.png";

});
```

La página sera similar a la que se muestra en la imagen debajo.

![iOS-Tabs](https://raw.githubusercontent.com/nishanil/Dev-Days-HOL/master/01%20Dev-Labs/screenshots/iOS-Tab-icons.png?token=AC9rtoWVK4eOTAWDV69qFcZyy9veMPJKks5X0mezwA%3D%3D)

### DependencyService
Vamos a utilizar la API nativa de Text to Speech para leer el texto de los usuarios. Como cada plataforma provee su propia API para **Text to Speech**, vamos a utilizar `DependencyService` para invocar la implementaciòn de cada plataforma desde el código compartido.

Desde la interfaz `ITextToSpeech`, definí el método `Speak()`. Abrí el archivo **XamarinAssemble\ITextToSpeech.cs** y añadí el siguiente código:

```csharp
public interface ITextToSpeech
{
    void Speak(string text);
}
```
Abrí el archivo **TextToSpeech.cs** en el proyecto Android **XamarinAssemble.Android** y añade lo siguiente:

```csharp
public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
{
    TextToSpeech speaker;
    string toSpeak;

    public TextToSpeechImplementation() { }

    public void Speak(string text)
    {
        var ctx = Forms.Context; // useful for many Android SDK features
        toSpeak = text;
        if (speaker == null)
        {
            speaker = new TextToSpeech(ctx, this);
        }
        else
        {
            var p = new Dictionary<string, string>();
            speaker.Speak(toSpeak, QueueMode.Flush, p);
        }
    }

    #region IOnInitListener implementation
    public void OnInit(OperationResult status)
    {
        if (status.Equals(OperationResult.Success))
        {
            var p = new Dictionary<string, string>();
            speaker.Speak(toSpeak, QueueMode.Flush, p);
        }
    }
    #endregion
}
```
Descomenta el atributo `assembly` (arriba del namespace) en el mismo archivo.

```csharp
[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplementation))]
```
Este atributo registra la clase como una implementación de la interfaz ITextToSpeech, lo que significa que podremos utilizar `DependencyService.Get<ITextToSpeech>()` desde el proyecto compartido para crear una instancia.

Para facilitarlo, la implementación de `TextToSpeech` para iOS ya se encuentra añadida. Podes mirar los archivos para ver como se realiza la implementación. 

Finalmente, llama a la implementación desde el proyecto compartido usando DependencyService. Abrí el archivo **XamarinAssemble\SessionDetailViewModel** y agrega lo siguiente en la inicialización del comando **SpeakCommand**. 

```csharp
DependencyService.Get<ITextToSpeech>().Speak($"Session {SessionName} presented by {SpeakerName} is on {Time}");
```

### Corre la aplicación!

Corre la aplicación y presiona el botón de hablar.

### CustomRenderers
Los `CustomRenderers` pueden ser utilizados para pequeños cambios de estilo o cambios sofisticados para cada plataforma. En esta aplicación, vamos a crear un botón custom llamado `SpeakButton` y añadiremos un Custom Renderer en el proyecto Android para mostrar una imagen junto al texto del mismo. 

Abrí el archivo **XamarinAssemble/Controls/SpeakButton.cs** y valida la existencia de una clase vacía que deriva de `Button`. Acorde a la simpleza de este workshop, lo mantendremos asi. 

Abrí la implementación del proyecto Android **XamarinAssemble.Android/Renderers/SpeakButtonRenderer.cs** y reemplaza el método `OnElementChanged()` para agregar la imagen al botón. Acá esta el código:

```csharp
protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
{
    base.OnElementChanged(e);

    if (Control != null)
    {
        Control.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.speakerphone, 0, 0);
        
    }
}
```
Ahora, abrí el archivo **XamarinAssemble\Views\SessionDetailPage.xaml** y reemplaza el botón existente para **Speak** por el de **SpeakButton** que se encuentra debajo.  

```xml
<local:SpeakButton
        Margin="0,10,0,0"
        Text="Speak" Command="{Binding SpeakCommand}" />
```
Compila y corre el proyecto para ver los cambios en el botón. iOS y Android no tienen implementaciones customizadas. Por ello, muestran controles de botones regulares. Sentite libre de modificar los Custom Renderers y añadirles tu creatividad.

## Wrapping Up!

Lo hicimos genial! Nuestra primer aplicación con Xamarin.Forms está corriendo!
En el próximo módulo nos vamos a conectar con Azure! 



