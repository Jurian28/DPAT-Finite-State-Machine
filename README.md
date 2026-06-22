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

## Unit tests

De tests staan in `Tests/` (xUnit) en dekken de kern-businesslogica af: de vier
diagram-validators. Elke test bouwt een minimaal diagram dat precies één regel isoleert,
draait de echte validator via `diagram.Accept(validator)` en controleert daarna
`IsValid` / `Errors`.

Hoe de tests werken is gevisualiseerd in `AKD/Tests/`:
- `TestFlow` — sequence diagram van de Arrange-Act-Assert-flow en de double dispatch via `Accept()`.
- `TestStructure` — class diagram van de test-harness (`ValidatorTests`, de `Fsm`-builder) en hoe die de Models en Validators aanspreekt.

### Runnen

```bash
dotnet test Tests/DPAT-Finite-State-Machine.Tests.csproj
```

### Wat iedere test doet

**DeterminismValidator** — controleert of een state deterministisch is

1. `Determinism_ReportsError_WhenTwoTransitionsShareTriggerAndGuard` — twee uitgaande transities met dezelfde trigger én guard → fout (niet-deterministisch).
2. `Determinism_IsValid_WhenOutgoingTransitionsHaveDistinctTriggers` — twee transities met verschillende triggers → geen fout (geen vals-positief).

**ReachableValidator** — controleert bereikbaarheid en dode transities

3. `Reachable_ReportsError_WhenStateCannotBeReachedFromInitial` — een losse state zonder inkomende transitie is onbereikbaar → fout.
4. `Reachable_TreatsSubstate_AsReachable_ThroughCompoundParent` — een substate zonder directe transitie is toch bereikbaar via zijn compound-parent → geen fout.
5. `Reachable_ReportsError_WhenConditionalTransitionIsShadowedByUnconditionalOne` — een guarded transitie naast een onvoorwaardelijke is dode code → fout.

**InitialFinalStateValidator** — regels rond start- en eindstates

6. `InitialFinal_ReportsError_WhenNoInitialStateExists` — diagram zonder initiële state → fout.
7. `InitialFinal_ReportsError_WhenMultipleInitialStatesExist` — meer dan één initiële state → fout.
8. `InitialFinal_ReportsError_WhenFinalStateHasOutgoingTransition` — een final-state mag niet verder schakelen → fout.

**TransitionTargetValidator** — controleert het doel van transities

9. `TransitionTarget_ReportsError_WhenTransitionTargetsUnknownState` — transitie naar een state die niet in het diagram zit → fout.
10. `TransitionTarget_ReportsError_WhenTransitionTargetsInitialState` — transitie terug naar een initiële state is verboden → fout.
