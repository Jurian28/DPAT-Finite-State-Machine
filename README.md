# DPAT Finite State Machine

## PlantUML-diagrammen renderen (Windows)

De class-diagrammen in `AKD/` hebben we al als `.svg` én `.png` meegeleverd — die hoef je dus
niet zelf te genereren om ze te bekijken.

PlantUML installeren is wél nodig voor het **beste resultaat tijdens het runnen van de
applicatie**: de grafische weergave (`GraphicalView`) genereert tijdens het draaien zelf een
`.puml` van de ingelezen FSM en rendert die naar een afbeelding. Zonder PlantUML (en Graphviz)
schrijft de app alleen het `.puml`-bestand weg en blijft de gerenderde afbeelding achterwege.

Zo maak je op Windows zelf een SVG of PNG van een `.puml`-bestand.

### Wat je moet installeren

1. **Java** (JRE of JDK 8+) — PlantUML draait op Java.
2. **Graphviz** — levert `dot`, nodig voor de layout van class-/state-diagrammen.
3. **PlantUML** — de tool zelf (`plantuml.jar` of via een package manager).

---

### Methode 1 — via winget (makkelijkst, Windows 10/11)

Open **PowerShell** en draai:

```powershell
winget install --id EclipseAdoptium.Temurin.21.JRE
winget install --id Graphviz.Graphviz
winget install --id PlantUML.PlantUML
```

Sluit PowerShell daarna en open 'm opnieuw (zodat de PATH ververst). Renderen:

```powershell
# PNG
plantuml -tpng pad\naar\diagram.puml

# SVG
plantuml -tsvg pad\naar\diagram.puml
```

De output (`diagram.png` / `diagram.svg`) komt naast het `.puml`-bestand te staan.

> Alternatief met **Chocolatey**: `choco install temurin graphviz plantuml`
> of met **Scoop**: `scoop install openjdk graphviz plantuml`
