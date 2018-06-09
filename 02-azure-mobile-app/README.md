# Azure Tablas Faciles

Durante el modulo anterior escribimos una aplicacion Xamarin.Forms que tomaba informacion expuesta en un servicio RESTful. En este modulo lo que vamos a estar haciendo es configurar Azure para poder usar Mobile Apps 

## Creacion de Azure Mobile Apps usando Tablas Faciles

Entrar a la siguiente url: http://portal.azure.com 

1. Una vez dentro del portal hacer click en **Crear nuevo recurso**, escribir en el buscador **mobile apps** para que salgan los resultados como en la imagen de abajo. 

![Buscar Mobile App](./images/01.BuscarMobileApp.PNG)

2. Luego, seleccionar **Mobile apps Quickstart**.

![Seleccionar Mobile App](./images/02.SeleccionarMobileApp.PNG)

3. Finalmente, oprimir **Crear** para que Azure cree el nuevo recurso.

![Mobile App QuickStart](./images/03.MobileAppQuickstart.PNG)

En el caso de que el usuario que estemos utilizando no tenga una suscripcion a Azure, nos va a pedir que obtengamos una.

![Suscripcion](./images/04.Suscripcion.PNG)

4. Se va a abrir una ventana con cuatro campos para configurar:

    **Nombre de la aplicacion:**
    Es un nombre unico que va a ser necesario para configurar el end point. 

    **Suscripcion:**
    Seleccionamos una suscripcion de la lista.

    **Grupo de recursos:**
    Seleccionamos **Crear uno nuevo** y escribimos el mismo nombre que la aplicacion.

    **Plan de App Service/Ubicacion:**
    Hacemos click en este campo y seleccionamos **Crear nuevo**, le damos un nombre unico y seleccionamos el plan F1 Free.

Por ultimo clickear en **Anclar al panel** y luego en **Crear**. Va a tardar entre unos 3 y 5 minutos en crear el nuevo recurso. 

![Creacion](./images/05.Creacion.PNG)

5. Si volvemos al Dashboard vamos a poder ver tanto el nuevo recurso como nuestra suscripcion a Azure.

![Dashboard](./images/06.Dashboard.PNG)

6. Seleccionar nuestro recurso y buscar en el menu lateral izquierdo **Tablas faciles**

![Dashboard](./images/07.OpcionTablasFaciles.PNG)

7. Haciendo click en **Agregar desde CSV** vamos a poder agregar una nueva tabla con un conjunto de datos. Asegurate de haber bajado de este repositorio los archivos speaker.csv y sessions.csv que estan en la carpeta Mock-Data

![Agregar CSV](./images/08.AgregarCSV.PNG)

## Testeando el endpoint

Ahora que ya hemos subido nuestros datos exitosamente a nuestro backend de Azure, podemos probar que este funcionando correctamente. Solo tenemos que copiar la url del Mobile Appsd Service que aparece en **Introduccion**, dicha opcion se encuentra en el menu lateral izquierdo.

![Test CSV](./images/09.TestRecurso.PNG)
