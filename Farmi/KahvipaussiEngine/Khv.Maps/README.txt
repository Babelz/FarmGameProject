- Karttojen käyttöönotto -
Käyttäjän tulee luoda Configuration tiedosto ja kertoa map managerille missä se sijaitsee alustuksen yhteydessä.
Kaikki pathit ja namespacet on oltava olemassa, tai poikkeus heitetään.

Esim 
MapManager mapManager = new MapManager(game: this, @"C:\Users\config.xml");

- Configurationfilen rakenne -
<ROOT>
  <MapPaths>
    <Path></Path>
  </MapPaths>

  <MapObjectNamespaces>
    <Namespace></Namespace>
  </MapObjectNamespaces>

  <MapComponentNamespaces>
    <Namespace></Namespace>
  </MapComponentNamespaces>
</ROOT>

- Mapobjectien reflectauksesta -
MapObjectin nimi tulee olla sama kuon itse luokan.

- Komponenttien reflectauksesta -
Komponenttien syntaksi on: [LayerinNimi] -[KomponentinNimi]([Parametri]:[Arvo]);

Esim 
TilePlatform1 -Moving(startX:0, endX:100);
TilePlatform2 -PointMover("tileplatform.pointmap");

Default konsu olioille jotka luodaan suoraan kartasta on
Konsu (KhvGame game, MapObjectArguments args) {
}