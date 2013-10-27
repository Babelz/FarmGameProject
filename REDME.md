TODO
# Farmi peli 0.1 alpha Kairatie Edition

### Timestep
* 1 pelimaailman minuutti = 1 IRL sekunti
* Päivä = 24 min

### Kalenteri huolehtii: 
* vuosista, 
* päivistä, 
* seasonista, 
* viikonpäivistä, 
* (omista) merkinnöistä, 
* merkkipäivistä ja 
* ajasta.


## Toimintoja 

### Kalenteri
Kalenterin pystyy pysäyttämään, jolloin aika ei enää juokse tai päivä vaihdu.

### World
World huolehtii kalenterista ja sen ajanlaskusta. Kun world sanoo, että skippaa päivä niin 
kalenteri skippaa sillon päivän.
 

## Kokonaisuudet

### Tilet
Tilet on staattisia, eikä niitä pitäisi koskaan vaihtaa. Rulelayer määrittelee vaan base collision, eli about maailman rajat ja kaupungin. Tileillä on tekstuuri tai animaatio. 


### Gameobjectit
Jokainen liikutettava olio on gameobject, eli toisin sanoen gameobject voi liikkua. Aidat, teleportit, eläimet, huonekalut ja kasvit ovat gameobjecteja. Rakennukset voivat olla gameobjecteja, joka antaa sen edun, että sitä voidaan liikuttaa vapaasti kunhan se ei collidaa keneenkään.  

#### Eläimet
Eläimet koostuvat **Animal** luokasta. Ne alustetaan AnimalDataSetillä, jossa on kaikki informaatio eläimelle, kuten sen behaviour, nimi, ikä yms. 

#### Rakennukset
Rakennukset ovat gameobjecteja. Ne koostuvat BuilidingDataSetistä, jossa on kaikki informaatio mitä ne tarvitsevat. 
```cs
class Building : DrawableGameObject {
	
}

class House : Building {
	
}

class Barn : Building {
	
}

class Shop : House {
	
}

```

### Filemanager
Luodaan filemanager joka helpottaa asioita ja hoitaa mm. tiedostojen **etsintää**, tiedostojen **luomista**, **poistamista**, **kopiointia**,
**siirtämistä** sekä **kansioiden luontia**. Muutamia apureita, jotka voisivat osottaa vaikka yleisimpiin paikkoihin, kuten 
pelin juureen, pelin content kansioon yms. 



### Farmin filepäätteet
* Kartat (.mp)
* Data setit (.dt)
* Save filet (.sv)
* Dbt (.db)
* Scriptit (.sc)

### Savedata

 * Eläimet, 
 * kasvit, 
 * eventit, 
 * NPC tiedot 
  * Kuinka paljon pitää pelaajasta
  * Eventit
  * World eventit

Pidetään muistissa niin kauan kun pelaaja käy tallentaan pelin. 
# !!!!!!! SAVE FILE TIETOKANTAA EI SÖRKITÄ IKINÄ ENNEN KU PELAAJA TALLENTAA PELIN !!!!!!

Tietokannasta ei poisteta ikinä mitään, vaan merkataan asioita poistettaviksi. Se antaa backwards compatibilityä ja pystyy korjaamaan mahdolliset bugit, koska juttu on vielä tallessa.

### Repot

Jokaselle datalle on oma repo joka on **readonly**. Repoja ovat mm. Npc, Animal ja Item data kollektiot.
Repot osaavat ladata itse itsensä, mutta ajonaikana niihin lisääminen tai niistä poistaminen on mahdotonta. Repot toimivat siis ikäänkuin tietovarastona.

TODO: miten se savettaminen?

Esimerkki kuvastettuna **pseudokoodilla**.
```cs
internal abstract class ReadonlyRepo<T> {
	protected readonly string reponame;

	protected List<T> items;

	public ReadonlyRepo(string reponame) {
		this.reponame = reponame;
	}

	// lataa kaiken tiedon
	public abstract void Load();
	public abstract T GetItem(Predicate<T> predicate);
} 
```

```cs
internal abstract class Repo<T> : ReadOnlyRepo<T> {
	public abstract void AddItem(T item);
	public abstract void RemoveItem(T item);
	public abstract void Save();
}
```


### Importterit ja exportterit

Jokaisen tallennettavan objectin tulee koostua ISaveablesta rajapinnasta. 

```cs
public interface ISaveable  {

	// Valmistelee olion tallennettavaksi, 
	// luo siitä XML dumppi valmiin version 
	// joka annetaan GameDataManagerille tallennettavaksi
	void Export(SaniExporter exporter);

	// Ottaa importterista olion omat tiedot 
	// ja asettaa ne sen fieldeihin
	void Import(SaniImporter impoter);
}
```

**Exportterilla** on lista, joka toimii ikäänkuin cachena johon oliot tallennetaan. Exportterilla ei ole pääsyä tiedostoihin. Kun exporter on dumpannut kaiken datan listaan, SaveDataManager huolehtii sen tallentamisen save filuun. 
**Importerilla** on avattuna XDocument, josta jokainen objekti voi hakea itselleen tarvittavat tiedot. Importerilla on readonly oikeudet tiedostoon. 