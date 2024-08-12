# Spraakassistent
## Opdrachtomschrijving

Een applicatie die, net als een home assistant, luistert naar een activatie zin/woord, en vervolgens gesproken instructies kan uitvoeren. 

Omdat dit een computerapplicatie is zou het waarschijnlijk neerkomen op dingen als volume controle, het opstarten van applicaties, een zoekopdracht uitvoeren, of van tevoren ingevoerde stappen uitvoeren.

De app zou dan een home scherm, 
setup/settings scherm, 
geschiedenis scherm, 
en "instructies" scherm hebben. 

Het instructies scherm is de plek waar dan custom uitvoerbare instructies ingevoerd kunnen worden. (Het is ook mogelijk dat alles via dit scherm ingevoerd moet worden. Afhankelijk van de ingebouwde OS componenten van C# en Maui)



## Input

Spraak instructies:
Wanneer het geluidsniveau stijgt activeert het herkennings systeem om te kijken of het activatie woord wordt gesproken (In dit geval "Hey Laptop")

Zodra het activatie woord is gesproken en herkend begint het systeem met actief luisteren naar sleutelwoorden die zijn toegevoegd aan de lijst met instructies.



Instructies toevoegen/verwijderen scherm:
Een sleutelwoord kan worden toegevoegd, vervolgens kunnen acties op de laptop aangewezen worden die uitgevoerd moeten worden wanneer het ingevoerde sleutelwoord wordt gesproken.

Deze instructies kunnen ook weer verwijderd worden.

## Output

Afhankelijk van instructies kunnen er allerlei verschillende vormen van output zijn, dit kunnen dingen zijn als het geluidsniveau dat aangepast wordt, een applicatie of webbrowser(met zoekterm of webpagina) die opgestart wordt, of een bestand dat geopend wordt.

## Klassendiagram

schets
