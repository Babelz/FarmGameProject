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

### Savedata

 * Eläimet, 
 * kasvit, 
 * eventit, 
 * NPC tiedot 
 	* Kuinka paljon pitää pelaajasta
 	* Eventit
 	* World eventit

Pidetään muistissa niin kauan kun pelaaja käy tallentaan pelin. 
#!!!!!!! SAVE FILE TIETOKANTAA EI SÖRKITÄ IKINÄ ENNEN KU PELAAJA TALLENTAA PELIN !!!!!!

Tietokannasta ei poisteta ikinä mitään, vaan merkataan asioita poistettaviksi. Se antaa backwards compatibilityä ja pystyy korjaamaan mahdolliset bugit, koska juttu on vielä tallessa. 


