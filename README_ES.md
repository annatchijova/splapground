# SLAPGROUND

[English](./README.md) · [Español](./README_ES.md) · [**Technical README**](./README_TECHNICAL.md) *(en inglés)*

### Un mal día de oficina, y tu escritorio real es donde lo descargás.

Tuviste el día que todos tuvimos alguna vez: la impresora se trabó, el mail no paraba, el teléfono no se callaba. SLAPGROUND es un juego de realidad mixta para Meta Quest, jugado sentado, donde esos mismos objetos —el despertador, la impresora, la nube de mails, el teléfono con cable— aparecen flotando sobre tu *escritorio real*, y los resolvés de la manera obvia: con las manos, sin controles.

![Concept art de SLAPGROUND: un escritorio en pleno caos, despertador e impresora explotando mientras las manos golpean a través de ellos](docs/concept-art/hero.png)
*Concept art generado con IA (Gemini/ChatGPT) — solo ilustrativo. Todavía no existe build; esto no es gameplay real.*

---

## Qué es

Agarrás un objeto con una mano, lo cacheteás con la otra, y mirás cómo reacciona: algo liviano rebota como loco, algo pesado necesita varios golpes antes de salir volando, un teléfono con cable se convierte en una especie de boleadora. Nada está guionado — el caos sale de cómo interactúan entre sí el peso, la fragilidad y la forma de los distintos objetos, y de cómo rebotan contra tu escritorio real.

![Cuatro estados del HUD: multiplicador de caos y contador de combo, rastro fantasma asincrónico activo, un momento de alto estrés con varios objetos en juego, y una reacción en cadena cruzando el escritorio](docs/concept-art/hud-states.jpeg)
*Concept art generado con IA — solo ilustrativo, no es una interacción capturada.*

Una sesión es corta a propósito: un par de minutos de caos creciente, rematados por diez segundos donde todo lo que queda en el escritorio es blanco libre — pegale a todo, lo más fuerte y rápido que puedas.

## Por qué volverías a jugar

Cualquiera puede dar una primera cachetada satisfactoria en segundos. Ser bueno de verdad lleva más tiempo: calcular un golpe para que un objeto rebote contra otro, encadenar varios objetos en un combo largo, aprender qué le hace cada objeto a cuál. Ahí vive el gancho competitivo — no un rival en vivo, sino el rastro fantasma de tu propia mejor partida o la de un amigo, un puntaje de caos por minuto, y un clip de tu mejor cadena, generado automáticamente, listo para mandar.

![Una reacción en cadena cinética: el impacto de un despertador dispara la explosión de una impresora que dispersa una nube de mails, con lectura en vivo de la física de cadena](docs/concept-art/chain-reaction.jpg)
*Concept art generado con IA — solo ilustrativo.*

![Leaderboard asincrónico y clip autogenerado de una mejor marca personal](docs/concept-art/async-leaderboard.jpeg)
*Concept art generado con IA — solo ilustrativo.*

## Qué tiene de distinto

| Experiencia típica de "romper cosas" en VR | SLAPGROUND |
|---|---|
| Una sala virtual genérica | Tu escritorio real, vía passthrough — los rebotes caen sobre tu mesa de verdad |
| La destrucción es toda la propuesta | La destrucción es la fantasía; el juego real son las combinaciones de propiedades de los objetos |
| Niveles guionados para dar profundidad | La profundidad emerge de un puñado de propiedades físicas combinables |
| Holograma de rival en vivo, o sin bucle competitivo | Un rastro tenue de una partida pasada — competitivo sin el ruido visual de los brazos de un rival en tiempo real |

## Dónde está esto ahora

**Esto es una hipótesis en validación, no un juego terminado.** Antes de construir nada de lo de arriba de verdad, lo primero que hay que probar en un Quest real es si se puede trackear de forma confiable una cachetada rápida de dos manos — nadie mostró públicamente eso resuelto todavía, ni siquiera en los dos títulos que Meta mismo señala como referencia de diseño hands-first. Esa investigación, el plan de validación completo, y cada decisión de diseño detrás están documentados en profundidad en el [Technical README](./README_TECHNICAL.md) (en inglés).

```
splapground/
├── README.md              # versión en inglés
├── README_ES.md            # este archivo
├── README_TECHNICAL.md     # arquitectura, protocolo de validación, investigación sourceada, decisiones
├── BRAINSTORM.md            # brainstorm original, sin editar
└── docs/
    └── concept-art/         # imágenes ilustrativas generadas con IA referenciadas arriba
```

Construido para la **Meta VR Start Developer Competition 2026** (track Gaming, hands-first, división New Experience — deadline 18 de noviembre de 2026). Para el protocolo de validación, la investigación competitiva detrás de las claims de arriba, las decisiones de arquitectura y los riesgos abiertos, ver el [Technical README](./README_TECHNICAL.md).
