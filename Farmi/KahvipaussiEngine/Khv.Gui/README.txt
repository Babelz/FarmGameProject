<!-- Khv.Gui -->

Khv.Gui on KhvEnginen osa joka sisältää käyttöliittymän luontiin liittyvät työkalut ja luokat.

<!-- Perusluokat -->

Tärkeimmät luokat kirjastossa ovat Control ja Container.

Control on kaikkien kontrollien äiti, jokainen kontrolli johdetaan siitä. Control on abstracti.
Jos käyttäjä haluaa luoda kustomeita listenereitä, hänen täytyy itse hookata ne perivässä luokassa.
Control lisää itseensä automaattisesti perittynä Mouse ja Button listenerit, vakiona se sisältää Control listenerin.

<!-- Control luokan tärkeimmät propertyt -->

- Position: Controllin oikea ja relatiivinen positioni parenttiin jos semmoinen on olemassa.
- ControlSize: Controllin koko.
- Control Parent: Kontrollin parent kontrolli.
- bool Enabled: Onko kontrolli enabloitu. Jos false, kontrolli ei voi päivittää itseään, jos true, sallii päivitykset.
- bool Visible: Onko kontrolli näkyvissä. Jos false, kontrolli ei voi piirtää itseään, jos true, sallii piirtämisen.
- bool HasFocus: Onko kontrollissa focus? Käytetään mm inputin lukemisen validoinnissa.
- ControlEventDispatcher ControlEventListener: control eventtien kuuntelija joka kuuntelee Visibility, Enabled ja Focus changed eventtejä.
- Index FocusIndex: Mikä on kontrollin focus indexi jos se lisätään IndexNavigaattor olioon?

<!-- Control luokan tärkeimmät metodit -->

- void ReAnchor: Ankkuroi kontrollin uudestaan parenttiin jos kontrolli on jonku childi.
- void Focus: Antaa kontrollille focuksen.
- void DeFocus: Poistaa focuksen kontrollilta.
- void Update: Päivittää kontrollin.
- void Draw: Kutsuu DrawEffects ja DrawControl metodeja vakiona. Public metodi jonka tarkoitus on piirtää kontrolli oikein.
- void DrawControl: Protected metodi jonka tarkoitus on piirtää kontrolli.
- void DrawEffects: Protected metodi jonka tarkoitus on piirtää kontrollin efektit.

Kun kontrollista peritään, voi koko piirto logiikan kirjoittaa uudelleen tai käyttää base määrettä.

Kun kontrollista peritään, voi koko updaten logiikan kirjoittaa uudelleen tai käyttää base määrtettä.

<!-- Container luoka -->

Container luokka on abstracti perusluokka kontrolleille jotka voivat omistaa
muita kontrolleja. Esimerkki implementaatioita ovat muunmuassa Window ja Panel luokat.

Container sisältää vakiona ControlManager luokan, joka hoitaa sen kontrollien wräppäyksen.

<!-- Container luokan propertyistä -->

Ainoat propertyt jotka vaikuttavat childeihin ovat Enable ja Visible.


