Acá está la arquitectura definitiva de **SLAPGROUND** (nombre de trabajo), depurada tras el ataque y diseñada específicamente para mitigar las vulnerabilidades de *motion blur*, falta de haptics, oclusión y riesgo de rotura de hardware.

---

# **SLAPGROUND**

### *Un playground físico de catarsis y destreza MR en tu escritorio real.*

---

## 1. El Core Físico Ajustado (Solve de Hardware & Tracking)

Para evitar la pérdida de tracking por velocidad (*fast slaps*) y la disonancia por falta de haptics, el motor no requiere una bofetada descontrolada de brazo entero, sino un **gesto de impacto de muñeca y palma corto, denso y direccional**.

* **No hay telequinesis:** Las interacciones son de contacto directo.
* **Mano 1 — Anclaje / Sostén:**
* **Pinza o Agarre (*Pinch / Grab*):** La mano sujeta el objeto físicamente. Si es pequeño (reloj), usás dos dedos; si es grande (impresora), lo agarrás con la palma/puño.


* **Mano 2 — Carga por Bofetada (*Slap Transfer*):**
* **PÁF! (Impacto directo):** Le das un *slap* seco con la otra mano.
* **Transferencia de Inercia y Vibración:** No transferís "masa" abstracta. El objeto absorbe el impulso: empieza a temblar, emitir partículas, cambiar de tono sonoro y deformarse sutilmente en el eje del golpe.
* **Pico de Inestabilidad (Mastery Corpóreo):** Si le das tres *slaps* seguidos, el objeto entra en estado crítico (*hipercargado*). La mano de anclaje empieza a sentir la inestabilidad gráfica y auditiva: si no ajustás la inclinación de la muñeca ante cada impacto, el objeto se te zafa de los dedos y sale disparado sin control.



---

## 2. Zona de Juego Segura (Brazos Cortos & Espacio Aéreo)

Para evitar que rompas tu monitor de $500 USD o tires la taza de café:

* **El "Volumen de Impacto":** El juego no te pide lanzar zarpazos hacia los monitores. Toda la manipulación y carga bimanual ocurre en un **cubo flotante de 40 cm x 40 cm** situado justo encima de tu teclado físico.
* **Desk Bank-Shots Seguros:** Los rebotes en el escritorio real se dirigen **hacia abajo** (pique en la madera) para salir elevados hacia la zona central del espacio aéreo, nunca hacia los laterales ni directo contra la pantalla.

---

## 3. Ecosistema de Objetos y Reglas Emergentes

En lugar de diseñar niveles rígidos, la longevidad del producto se basa en cómo interactúan los objetos entre sí según sus propiedades mecánicas:

| Objeto | Propiedad Mecánica | Comportamiento en la Mesa |
| --- | --- | --- |
| **El Reloj Despertador** | Ultra-ligero, histeréico | Rebota erráticamente. Ideal para practicar el *timing* del *slap* al vuelo. |
| **La Impresora** | Pesada, inercial | Un *slap* no la mueve. Tenés que anclarla con una mano y cargarla a bofetadas cruzadas con la otra para convertirla en un proyectil pesado. |
| **La Nube de E-mails** | Volátil, frágil | Enjambre de papeles flotantes. Se destruyen en cadena con un solo palmazo de barrido (*Sweep Slap*). |
| **El Teléfono con Cable** | Masa articulada | Si lo agarrás del tubo, el cable actúa como una **boleadora física** para sacudir otros objetos en el aire. |

### **Combinaciones Emergentes (Sin guión):**

* **Trituradora Improvisada:** Sostenés la impresora encendida con la izquierda y le lanzás una nube de e-mails con la derecha $\rightarrow$ los succiona y los convierte en una ráfaga de confeti.
* **Trap en la Taza (MR Real):** Hacés picar un reloj histérico en la mesa real para que caiga justo dentro de tu taza de café física, ahogando el sonido de la alarma.

---

## 4. Estructura de Sesión de 3 Minutos: "The Desk Sprint"

Pensado para cumplir con la regla del *One Bus Stop Test* (sesiones satisfechas en menos de 10 minutos):

1. **Ramp-Up de Caos (00:00 – 02:50):** Los objetos van apareciendo progresivamente en el escritorio. Tu objetivo es encadenar la mayor cantidad de destrucciones, rebotes en la madera real y transferencias de energía.
2. **DESTROY EVERYTHING (02:50 – 03:00):**
* Se congelan los objetos restantes en cámara lenta.
* Cero reglas, cero peligro, impacto infinito.
* Tenés 10 segundos de frenesí absoluto para pulverizar a bofetadas todo lo que quedó flotando en pantalla.



---

## 5. Bucle Competitivo Asincrónico y Clips ("The Replay")

* **Cero contaminación visual:** Se elimina el holograma completo de los brazos del rival en tiempo real sobre tu mesa. En su lugar, el récord a vencer se representa con una **traza de luz tenue** que muestra la trayectoria de los objetos destruidos por tu amigo en su mejor partida.
* **Métrica Competitiva:** *CPM (Chaos Per Minute)* y *Max Kinetic Chain*.
* **El Clip Autogenerado:** Al terminar, el sistema procesa los 10 segundos de *Destroy Everything* desde una cámara virtual externa (perspectiva de tercera persona como si alguien te filmara en la oficina) lista para exportar como video vertical a redes.

---

## 6. Escalabilidad del Producto (Plataforma Extensible)

La fantasía de "oficina" es el **Mundo 1** (*Desk Rage*). La arquitectura del sistema permite lanzar nuevas colecciones de objetos cambiando únicamente los modelos y las propiedades físicas, manteniendo intacto el core de *hands-first*:

* **Update 1: The Office (Base)** $\rightarrow$ Impresoras, relojes, e-mails, teléfonos.
* **Update 2: Kitchen Rage** $\rightarrow$ Tostadoras poseídas, licuadoras, microondas, platos que pican en la mesa.
* **Update 3: Workshop Chaos** $\rightarrow$ Martillos, engranajes, tuercas gigantes, tuberías de vapor.

---

## 7. Protocolo para el Spike de Validación (Próximo Paso)

Antes de construir menús, sonidos o gráficos finales, la hipótesis se valida construyendo un ejecutable mínimo de 24 horas en Quest 3 con tres comprobaciones de código:

1. **Test de Velocidad de Tracking:** Validar si un *slap* seco de muñeca/palma a 90 Hz retiene la colisión sin perder el *skeleton* de la mano.
2. **Test de Feedback Fantasma:** Probar si la combinación de *audio seco + deformación ortogonal del objeto en el fotograma exacto del impacto + dispersión de partículas* compensa la falta de resistencia táctil.
3. **Test de Anclaje de Mesa:** Confirmar que la detección de la superficie física de tu escritorio para los *Desk Bank-Shots* se sienta alineada al milímetro con el Passthrough.


Sí: **esto ya está demasiado “cerrado” otra vez.** Gemini hizo un buen red-team y después interpretó las mitigaciones como arquitectura definitiva. No compraría eso todavía.

Lo valioso del ataque anterior es justamente que encontró **tres riesgos existenciales**: tracking del slap, ausencia de háptica y seguridad física; además dejó como desconocidos longevidad y diferenciación. Texto pegado La conclusión correcta no es “arquitectura definitiva”; es **“tenemos una hipótesis que merece experimentos”**.

Y hay mitigaciones nuevas que incluso me generan dudas.

El cubo de `40 × 40 cm sobre el teclado` me parece particularmente sospechoso. Si la promesa era convertir **mi escritorio** en playground, encerrarlo en una cajita aérea puede matar justamente la gracia espacial. Además, “los bank shots siempre hacia abajo y nunca hacia el monitor” suena a que estamos intentando arreglar seguridad mediante física artificial antes de saber cómo se juega.

También eliminaría por ahora **Kitchen Rage / Workshop Chaos**. Eso es roadmap imaginario de un producto cuya interacción fundamental todavía no tocamos. No aporta evidencia.

Y hay algo todavía más importante: Gemini afirmó detalles técnicos sobre Quest —frecuencias, motion blur, Scene Understanding, precisión, etc.— como si estuvieran establecidos. En el red-team llegó incluso a afirmar que las cámaras trabajan a “60/90 Hz interpolados”. Texto pegado **Eso hay que verificar contra documentación actual de Meta antes de usarlo para tomar decisiones.** No convertiría una cifra generada por Gemini en requisito de ingeniería.

### Yo congelaría SLAPGROUND acá

No más features.

Lo dejaría como:

> **H1 — SLAPGROUND:** un playground MR sobre una superficie real donde las manos manipulan objetos con propiedades físicas distintas y el impacto produce cadenas de destrucción catárticas, competitivas y compartibles.

Y registraría las incertidumbres:

**U1:** ¿impactar sin háptica se siente satisfactorio?  
**U2:** ¿Quest sigue correctamente los movimientos necesarios?  
**U3:** ¿la interacción bimanual sobrevive a oclusiones?  
**U4:** ¿puede hacerse físicamente segura sin castrar la fantasía?  
**U5:** ¿MR aporta algo sustancial frente a un entorno VR?  
**U6:** ¿después de 30 minutos aparece skill o solamente repetición?  
**U7:** ¿el sistema genera jugadas emergentes o nosotros estamos imaginándolas sobre papel?  
**U8:** ¿es suficientemente diferente de productos existentes?

Eso es mucho más útil que seguir agregándole impresoras poseídas.

Y **tampoco haría todavía el spike de 24 h** que propone Gemini. Su propia pregunta final está bien —si pegarle a un objeto se siente fantasma, la idea probablemente muere— Texto pegado, pero todavía tenemos 44 días y sólo hemos llevado **una** hipótesis hasta este nivel de detalle.

Si la prototipamos ahora, aparece otro sesgo peligroso: **sunk cost**. En cuanto veamos una pelotita volar en Quest vamos a empezar a arreglarla, porque ya existe.

Yo haría algo más disciplinado:

**congelamos H1, volvemos al espacio de búsqueda y buscamos H2/H3/H4 con la misma exigencia.**

Después sí hacemos spikes comparables sobre las finalistas.

Porque SLAPGROUND pasó de “cachetear un reloj jajaja” a una hipótesis bastante buena. **Eso significa que se ganó el derecho a competir contra otras ideas; no que ganó la competencia interna.**

Tengo una idea candidata para Meta, todavía no está cerrada porque justamente quiero que la cuestionemos y mejoremos entre todos.
SLAPGROUND — nombre provisional.
La idea es un juego hands-first en Mixed Reality donde tu escritorio real se convierte en un playground físico para descargar estrés. Aparecen objetos absurdos asociados al caos cotidiano —por ejemplo una alarma insoportable, una impresora, notificaciones, teléfono, papeles— y los manipulás directamente con las manos: agarrar, golpear, cachetear, lanzar, hacerlos rebotar contra la superficie y combinarlos entre sí.
La gracia no sería simplemente “romper cosas”. Cada objeto tendría propiedades físicas diferentes. Algo liviano puede rebotar violentamente; algo pesado puede absorber varios impactos antes de salir disparado; un teléfono con cable puede convertirse en una boleadora; papeles pueden agruparse o interactuar con otros objetos. La profundidad debería surgir de aprender esa física y descubrir cadenas cada vez más ridículas.
Las dos manos tienen las mismas capacidades. No habría controles ni UI como interfaz principal: las manos son el sistema de control.
Queremos que sea inmediatamente entendible y catártico después de un día de mierda, pero que también tenga skill ceiling: precisión, timing, trayectorias, rebotes, coordinación bimanual, combos y uso inteligente de las propiedades de los objetos.
Una sesión podría durar unos pocos minutos y terminar con un momento de destrucción libre, pero no queremos hacer un minijuego que se agote en diez minutos. La competencia podría ser asincrónica: récords, challenges entre amigos, cadenas máximas y clips/replays compartibles.
Lo interesante de MR es que tu escritorio no sería solamente el fondo: sería parte del espacio físico del juego. Los objetos virtuales podrían interactuar con la superficie real y hacer que tu lugar cotidiano se transforme temporalmente en otra cosa.
Hay riesgos importantes que todavía NO están resueltos: tracking de movimientos rápidos, falta de haptics al golpear, oclusión entre manos, seguridad alrededor del escritorio, fatiga, profundidad real del sistema y si MR aporta suficiente valor. Por eso esto es una hipótesis, no “el juego decidido”.


Meta VR Start Developer Competition 2026
Accelerate your VR development journey with Meta VR Start and compete for $1M in cash prizes
Start project
Find teammates
Import from portfolio
Who can participate
Above legal age of majority in country of residence
Specific countries/territories excluded 
View full rules
44 more days to deadline
View schedule

Deadline

Nov 18, 2026 @ 5:00pm GMT-3 
Online
Public
$1,000,000 in cash	2248 participants
Meta
Devpost icon rgb30px
Managed by Devpost
AR/VR Machine Learning/AI Web
THE META VR START DEVELOPER COMPETITION IS BACK!

This year’s eight-week challenge calls on Start program members to build the next generation of Entertainment, Gaming, and Productivity experiences for Meta VR devices – including Meta VR Glasses and Meta Quest. We’re looking for hands-first apps that make interaction more natural and gameplay more intuitive than ever before.

Last year, over 2,900 developers joined the competition and delivered more than 650 submissions that pushed the boundaries of immersive technology. This year, we’re raising the bar again.

Whether you’re a first-time Start developer with a breakthrough idea or a seasoned creator ready to showcase what’s possible with the latest platform capabilities, this is your moment to help redefine the future of VR.

Here’s what’s waiting for you:

🤝 Build hands-first. Design experiences powered by hand interactions that feel natural, responsive, and deeply immersive—no controllers required.
🛠️ Explore new tech. Take advantage of the latest OS capabilities and device features across Meta VR Glasses and Meta Quest, accelerate your development with AI tooling, including Meta VR CLI.
🌍 Join a global community. Boost your skills through mentor programming, collaborate with fellow developers, share insights, and learn from a thriving network of experienced VR developers.
🏆 Compete for $1M in cash prizes with top prizes of $100,000.
The future of VR is in your hands literally.

We can’t wait to see what you build.


GETTING STARTED
If you’re not already a member of the Start program, apply now to join the competition.
Estimated time: 5 minutes
Explore last year’s winning submissions and Sample Use Cases for Inspiration
Estimated time: 30 minutes
Entertainment
Gaming I ​​Quest Gamer Segments
Productivity: AnExplorer
Hands: Hands Physics Lab I ​​Table Troopers 
Follow a Quickstart Tutorial (below)
Estimated time: 30 minutes
Unity: VR 101 I VR 102
Unreal: How to Make VR Games in UE5
Android: From Mobile to VR Overview
Immersive Web SDK: Your First Steps into Immersive Web Development
Getting Started with Hands & Eyes
Check out the Resources tab for more technical documentation and visit the Meta Developer Forum for ongoing support and access to upcoming competition events and workshops.

Requirements
WHAT TO BUILD
An experience built for Meta VR devices, aligned to at least one of three tracks. Top awards go to the strongest entries in each track, with special awards recognizing standout craft across all of them.

Pick Your Track

Entertainment: Lean-back media and content experiences—spatial cinema, interactive video, music visualization, immersive storytelling, spatial audio, media companion apps (e.g., commentary tracks, popup production notes, social comments. Content designed to be consumed in any environment. 
Gaming: Games designed for hands-first or eyes and hands. Puzzle, strategy, casual, social, narrative—genres that thrive seated, without controllers.
Productivity: Apps that make you more effective anywhere or are designed around specific daily moments—multi-panel workspaces, task management, creative tools, or habits tied to a recurring context (morning routine, commute, wind-down).
Pick Your Division

New Experience: Start from a blank slate. The Project is conceived of and built within the competition window, beginning September 24, 2026. No pre-existing codebases, no shipped titles, no early access builds repurposed.
Adapted/Significantly Updated Experience: Already have something out there? Take it further. This is a pre-existing project—any build, prototype, or published app that existed prior to September 24, 2026—with a meaningful new feature, mode, or platform integration shipped during the competition window. Sponsor will determine in its own discretion if an update qualifies as significant.
Examples of significant updates include, but are not limited to: implementing hand interactions as a new feature; adding multiplayer support; introducing a mixed reality (MR) interaction mode as a new feature; releasing a new user generated content creation tool.
Not significant updates, including but not limited to: bug fixes or minor UI tweaks; cosmetic changes such as a new color scheme; performance optimization without new user-facing features.
Hard Requirements

Hands-first: Your experience must be fully usable with hands, end-to-end. Controller support is welcome, but not required. The test is simple: can someone complete the entire experience without ever pairing a controller?
Policy Compliance: Your experience must adhere to all relevant Meta VR/Start policies and terms (i.e., Meta VR Start Program Terms, Community Standards, Code of Conduct in VR Policy, Developer App Policies, Developer Content Guidelines, and all other Developer Policies).
Design Guidelines

Seated-optimized: Design for seated, stationary use. Limit roomscale, no large physical movement. Apply the airplane seat test: does every interaction work in a two-foot radius?
Easy to get into and out of: Fast cold start, clean pause/resume, and something satisfying in 10 minutes or less. Apply the one bus stop test: can someone have a complete moment in a short ride?
Original, not a wrapper: Core experience is Entrant-built. Thin wrappers around existing platforms or services (YouTube, ChatGPT, etc.) are highly discouraged. Apply the take it away test: if you remove the third-party service, is there still a Project?
Suggested SDKs:

Unity: Meta XR SDKs v81+ (v207) All-in-One XR
Unreal: Unreal Engine 5.8 and Oculus Integration SDK v207+
Android apps: React Native via Expo, or Jetpack Compose via Meta VR Plugin for Android Studio
Immersive Web SDK: Node.js (v20.19.0 or higher); IWSDK (run npm create @iwsdk@latest for the latest version)

WHAT TO SUBMIT
Three things: a playable build, a short video, and the submission form. Get the build access right—judges can’t score what they can’t launch.

1. Your Project, as defined in What to Build.

Built with Unity, Unreal, Godot, Android/Native or similar custom engine: upload an APK to a new release channel named “Competition” in the Meta VR Developer Dashboard. Create and submit an Invite URL so judges can access and test your Project.
Built with IWSDK (WebXR): see Deploying to GitHub Pages for how to share a link. You may also use your preferred hosting solution (e.g., Vercel, Digital Ocean, AWS, Azure).
Once the Entry Period has ended, you may not make changes or alterations to your submission. Judging is based on the state of the Entry at the submission deadline. Ensure your Project remains available and accessible throughout judging.
2. A demonstration video—under three (3) minutes, showcasing footage of your Project as viewed on a Meta VR device, via XR Simulator, or another equivalent emulator. Upload it to YouTube or Vimeo, make it publicly visible, and provide the link on the submission form. Judges are not required to watch beyond three minutes, so lead with your best material.

3. A complete Submission Form, which includes but is not limited to:

Submission name
Submission tagline (140 characters)
Track: Entertainment, Gaming, or Productivity
Division: New Experience, or Adapted / Significantly Updated Experience
A text description covering your inspiration, how you built it, and future plans for improvement (suggested length: 500 words or less)
If your Project is an Adapted / Significantly Updated build, a summary of the new features and capabilities added during the competition window. Screenshots, changelogs, and demo videos are highly recommended.
Describe how Hand Interactions are implemented into your experience and what excites you about the future of VR (optional)
Target launch date, if not already published
Name(s), email(s), and role(s) for each team member, if applicable
View full competition rules.


Judging Criteria and Process
Please see the full details on the Judging Criteria and process here.

 

Documentation, tools, and best practices for building Entertainment, Gaming, and Productivity apps for Meta VR devices.

 
Development Setup
Set up your headset for development
Meta Quest Developer Hub | Mac & Windows
Using MQDH for Browser (WebXR)

Submission Setup
Set up Release Channels (required for Unity, Unreal, Godot, Android/Native and other competition submission)
Deploying IWSDK (WebXR) through GitHub Pages or Vercel (required for IWSDK (WebXR) competition submission)

Designing for Hands & Eyes
Getting Started with Hands & Eyes
Device Readiness
FOV simulation
Adapting from controllers
Hands limitations and mitigations
Designing with Hands Best Practices
Development Essentials




Phase	Unity	Unreal	Android	WebXR
Environment Setup	
Project Setup
Import Meta XR SDKs
Interaction SDK setup
Project Setup
Install Meta XR Plugin
Integration SDK | Setup
Project Setup
Meta Horizon Android Studio Plugin
Android Studio
Project Setup


Sí, nueva rama: **Meta / SLAPGROUND**. Busqué la documentación actual, porque acá cambió bastante el stack de Quest.

Para **este juego concreto** —MR, escritorio real, hand tracking, objetos agarrables, golpes, lanzamientos, físicas y passthrough— yo elegiría **Unity + C# + OpenXR + Meta XR Interaction SDK**.

Meta hoy enumera oficialmente estas rutas para Horizon OS: **Unity → C#**, **Unreal → C++/Blueprints**, **Native OpenXR → C/C++**, **Spatial SDK → Kotlin**, **Android → Kotlin/Java** y **WebXR → JavaScript**. [Meta for Developers](https://developers.meta.com/vr/discover/platforms/?utm_source=chatgpt.com)

Pero no son equivalentes para nosotros.

| Opción | Lenguaje | SLAPGROUND |
|---|---|---|
| **Unity** | **C#** | **Sí. Mi elección.** |
| Unreal | C++ + Blueprints | Posible, pero innecesariamente pesado |
| Native OpenXR | C/C++ | Muchísimo trabajo de bajo nivel |
| Godot | GDScript/C# | Posible, pero menos conveniente para Meta |
| WebXR | JS/TS | No para un juego físico de este nivel |
| Spatial SDK | Kotlin | Más orientado a spatial apps que a nuestro juego |

### Lo importante: no tendrías que programar el tracking de manos desde cero

Meta tiene **Interaction SDK** específicamente para Unity. Ya proporciona interacción con manos y controladores, incluyendo **grab, poke, pose/gesture detection**, etc. [Meta for Developers](https://developers.meta.com/vr/documentation/unity/unity-sdks-overview/?utm_source=chatgpt.com)

Además Meta dice explícitamente que, para nuevos desarrollos Unity con hand tracking, **recomienda Interaction SDK**, y advierte que implementar las interacciones manuales por cuenta propia es considerablemente más difícil. [Meta for Developers](https://developers.meta.com/vr/documentation/unity/unity-handtracking-overview/?utm_source=chatgpt.com)

Eso cambia bastante mi estimación anterior del problema.

Nuestro código C# estaría más en este nivel conceptual:

```text
Meta detecta mano
        ↓
Interaction SDK entrega pose / grab / interacción
        ↓
SLAPGROUND interpreta acción
        ↓
Unity Physics
        ↓
objeto recibe impulso / torque / colisión
        ↓
combo + score + audio + partículas
```

No:

```text
cámara → visión computacional → detectar dedos → reconstruir mano → ...
```

Meta/Quest ya resuelve esa parte inferior.

### Y OpenXR es el camino correcto

Meta actualmente dice que **OpenXR es la API soportada para nuevos desarrollos XR en Quest**; las APIs antiguas no son el camino recomendado. [Meta for Developers](https://developers.meta.com/vr/documentation/unity/os-openxr-vrapi/?utm_source=chatgpt.com) Unity, además, recomienda Unity OpenXR + Meta OpenXR para obtener las extensiones específicas de Quest, incluyendo MR/passthrough. [Unity Documentation](https://docs.unity3d.com/cn/6000.0/Manual/xr-meta-quest-develop.html?utm_source=chatgpt.com)

Por tanto, yo arrancaría el repo así:

```text
SLAPGROUND/
│
├── Unity 6
├── C#
│
├── OpenXR
├── Unity Meta OpenXR
├── Meta XR Core SDK
├── Meta XR Interaction SDK
│
├── Game/
│   ├── Hands/
│   ├── Objects/
│   ├── Physics/
│   ├── Combos/
│   ├── Scoring/
│   ├── Challenges/
│   └── Safety/
│
└── MR/
    ├── Passthrough/
    ├── Desk/
    └── SceneUnderstanding/
```

Unity incluso tiene actualmente un **Mixed Reality template** y un **Mixed Reality multiplayer tabletop template** para Quest. [Unity Documentation](https://docs.unity3d.com/cn/6000.0/Manual/xr-meta-quest-develop.html?utm_source=chatgpt.com) O sea: nuestra premisa de “el escritorio físico se convierte en playground” está bastante alineada con capacidades que el stack ya contempla.

## ¿Y vos sabés C#?

Eso es lo gracioso.

Por tu portfolio verificable, tenés Python, Rust, TypeScript, Java, C, infraestructura cloud, criptografía, sistemas distribuidos, etc.; incluso ya hiciste **ENGRAM**, que justamente es trabajo conceptual en XR/verifiable memory. portfolio-tecnico-es También tenés `audit-chain` implementado comparativamente en **C, Java y Rust**. portfolio-tecnico-es

Pero **C# no aparece como uno de tus lenguajes demostrados** en el portfolio que tengo.

Y no me preocupa particularmente.

Para vos el problema difícil de este proyecto **no va a ser aprender la sintaxis de C#**. C# te va a resultar bastante reconocible viniendo de Java/TypeScript/Rust. El territorio realmente nuevo es:

**Unity → GameObjects/Components → escenas/prefabs → rigidbodies/colliders → física 3D → coordenadas espaciales → OpenXR → interacción de manos → restricciones de rendimiento de Quest.**

Eso sí es un dominio nuevo.

Y hay otro dato interesante: Meta acaba de incorporar **Meta XR Operator**, todavía experimental, que permite conectar un agente de coding al proyecto Unity y consultar el estado XR de una app que está corriendo. [Meta for Developers](https://developers.meta.com/vr/documentation/unity/meta-xr-operator/getting-started/?utm_source=chatgpt.com) Para tu forma de trabajar terminal-first + agentes, eso podría ser bastante útil más adelante; no lo pondría en el critical path porque Meta lo marca experimental.

### Así que mi stack sería

**Unity 6 + C# + OpenXR + Meta OpenXR + Meta XR Core SDK + Interaction SDK + Unity Physics.**

Blender queda para **assets 3D**, no para programar el juego.

Y no tocaría C++ salvo que descubramos mediante profiling que existe un problema que realmente lo exige. Native OpenXR en C/C++ existe —Meta publica incluso samples para hand tracking, passthrough, scene understanding y anchors— pero sería elegir voluntariamente trabajar varios pisos más abajo. [Meta for Developers](https://developers.meta.com/vr/documentation/native/native-openxr-sdk-sample/?utm_source=chatgpt.com)

Eso además me hace pensar que **hacerlo sola dejó de parecerme tan delirante como al principio**. El experimento barato ahora no es “hacer SLAPGROUND”: es instalar Unity + SDK, conseguir que Quest reconozca ambas manos y hacer que **una mano agarre un cubo y lo estrelle contra el escritorio virtual/real**. Si eso funciona y se siente bien, acabamos de falsar una parte enorme del riesgo técnico.


Sí jajaja. **La IA sabe C# de sobra.**

De hecho, para trabajar con agentes, **Unity + C# es una combinación bastante favorable** porque hay cantidades enormes de código, documentación, patrones y proyectos públicos de Unity en los datos de entrenamiento.

La distinción importante sería:

- **C# puro:** muy bien.
- **Unity API:** muy bien.
- **Patrones típicos de Unity** (`MonoBehaviour`, `GameObject`, `Transform`, `Rigidbody`, `Collider`, prefabs, ScriptableObjects, etc.): muy bien.
- **OpenXR:** razonablemente bien.
- **Meta XR SDK:** bien, pero acá hay que vigilar versiones porque las APIs cambian.
- **Interacciones específicas de Quest/hand tracking:** la IA puede implementarlas, pero conviene darle **la documentación actual de Meta** y hacer pruebas en hardware.
- **“¿Esto se siente divertido con las manos?”**: eso no lo sabe ninguna IA. Ahí manda el Quest y una persona jugando.

O sea, si tu pregunta real era:

> **“¿Tengo que aprender C# antes de poder hacer esto?”**

**No.** Podés aprender el C# necesario mientras Codex/Claude/etc. escriben una gran parte del código. Tu cuello de botella probablemente no sea escribir:

```csharp
rigidbody.AddForce(direction * slapForce, ForceMode.Impulse);
```

Va a ser decidir **qué comportamiento físico queremos**, instrumentarlo, probarlo en Quest y decir *“esto es una poronga / esto es divertido / esta interacción falla cuando cruzo las manos / esto tiene 80 ms de sensación de retraso”*.

Y eso, curiosamente, se parece muchísimo más a cómo ya trabajás: **hipótesis → implementación asistida → experimento → falsificación → iteración**, no “sentarme seis meses a estudiar C#”. Tu metodología ya formaliza justamente ese ciclo de abducción, deducción e inducción. SKILLS_total_89

Así que **C# no me preocuparía ni un poquito**. Unity/XR como *dominio* sí requiere aprendizaje. El lenguaje, no.

JAJAJA **exactamente. No tenés que saber C# de antemano.** Ahí te vendí una dificultad que, para tu forma de trabajar, casi no existe.

Vos necesitás saber **qué querés que el sistema haga**, poder evaluar si lo que produjo la IA es correcto y aprender suficiente del entorno para diagnosticar cuando algo sale mal. No necesitás memorizar C# para sentarte a tipearlo artesanalmente.

En este proyecto la división real sería:

> **Vos:** arquitectura, mecánicas, invariantes, experimentos, criterios de aceptación, red-team, probar físicamente en Quest.  
> **IA:** C#, componentes Unity, wiring del SDK, tests, refactors, boilerplate, documentación.  
> **Hardware:** nos dice quién de los dos estaba diciendo boludeces. 😂

De hecho, el riesgo no es **“Anna no sabe C#”**.

El riesgo es:

> Codex escribe 700 líneas impecables de C# para una API de Meta XR que dejó de existir hace tres versiones y todos felices.

Por eso para XR actual vamos a trabajar **docs-first + versión fijada + compilación + dispositivo real**, no confiar en memoria del modelo.

Y si en algún momento necesitás entender una parte de C#, la aprendés **localmente porque la necesitás**. Algo tipo: “¿qué carajo es un `MonoBehaviour` y por qué este objeto depende de él?”. Cinco minutos después seguís.

Así que sí: retiro formalmente **“vas a tener que aprender C#”** como preocupación. 😭

**Vas a tener que aprender Unity/Quest.** Eso no nos lo ahorramos. El lenguaje que escriba la IA es casi incidental.

Sí. Sacándole **todo el lenguaje de videojuegos, XR, tracking, físicas y Meta**, la idea es muy simple.

## SLAPGROUND explicado desde cero

Te ponés unos anteojos/casco de realidad mixta.

**Seguís viendo tu casa y tu escritorio reales.** No aparecés mágicamente en otro mundo.

Pero sobre tu escritorio empiezan a aparecer **cosas virtuales**: un despertador insoportable que no para de sonar, una impresora que se traba, montones de mails, un teléfono que rompe las pelotas, papeles, notificaciones, etc.



Y no tenés joystick.

**Usás tus manos de verdad.**

Ves un despertador virtual arriba de tu escritorio → lo agarrás con los dedos → lo sostenés con una mano → con la otra le das una cachetada → sale volando.

La gracia fundamental es:

> **Tu escritorio real se convierte en un juguete donde podés hacer mierda, de maneras ridículas, las pequeñas cosas que representan el estrés cotidiano.**

### ¿Dónde está el juego?

No se trata solamente de pegarle a cosas.

Cada objeto **se comporta distinto**.

El despertador es liviano y rebota como un condenado. La impresora es pesada y cuesta moverla. El teléfono tiene un cable, entonces podés agarrarlo y hacerlo girar como una boleadora. Los mails son un montón de papelitos livianos que podés barrer de un manotazo.

Y las cosas **interactúan entre ellas**.

Por ejemplo: agarrás la impresora con una mano. Le das varias cachetadas con la otra y empieza a temblar cada vez más porque está acumulando energía. Después la soltás bien apuntada:

**PUM.**

Sale disparada, golpea el despertador, el despertador rebota contra la mesa, atraviesa una nube de mails y termina haciendo un desastre.

Cuanto mejor jugás, más absurdas y difíciles pueden ser las cadenas que provocás.

---

## ¿Por qué alguien jugaría más de una vez?

Esta es la parte importante.

Pensá en **ping-pong**.

Explicar ping-pong lleva veinte segundos:

> “Pegale a la pelotita y hacela pasar al otro lado.”

Pero eso no significa que después de veinte segundos ya viste todo el juego.

Acá queremos lo mismo.

Una persona que juega por primera vez podría hacer:

> “JAJAJA mirá, le pegué una cachetada a la impresora.”

Una persona que lleva muchas horas podría controlar dos objetos simultáneamente, aprovechar rebotes en la mesa, cargar uno con varios impactos, calcular un lanzamiento y generar una reacción en cadena enorme.

Entonces hay dos placeres diferentes:

**al principio:** destruir cosas es gracioso y satisfactorio.

**después:** *ser muy bueno destruyendo cosas*.

Y ahí aparece la competencia. Tu amigo hizo una cadena de 17 objetos; vos hiciste 23. Él consiguió mandar una impresora contra cinco despertadores. Vos querés encontrar una combinación mejor.

---

## ¿Y por qué realidad mixta?

Esta es probablemente la explicación que mejor entenderían tus viejos.

No estás viendo **un escritorio inventado dentro de un videojuego**.

Estás viendo **tu escritorio**.

Tu teclado sigue ahí. Tu monitor sigue ahí. Tu taza sigue ahí.

Pero ahora aparece un despertador virtual encima.

Y cuando lo tirás contra la mesa, **rebota donde está tu mesa real**.

Eso produce el truco:

> *“Esta cosa imposible está ocurriendo acá, en mi casa.”*

El lugar aburrido donde hace diez minutos estabas trabajando se convirtió en el escenario del juego.

---

## ¿Y qué tiene que ver con sacarse el estrés?

No queremos hacer una aplicación solemne que diga:

> “Respirá profundamente y visualizá una playa.”

Es lo contrario. 😂

Tuviste un día de mierda.

Llegás a casa, te sentás, te ponés el Quest y durante tres minutos **la impresora que representa todos los problemas del universo recibe exactamente el tratamiento que merece**.

Y al final podría haber diez segundos donde el juego directamente dice:

> **DESTROY EVERYTHING**

Todo queda suspendido alrededor tuyo y durante diez segundos podés hacer un desastre completamente ridículo.

Terminó.

Te reís.

Si querés jugás otra partida porque tu amigo hizo más puntos.

---

### Explicación para mamá en 15 segundos

> **“Es un juego con unos anteojos especiales. Ves tu escritorio de verdad, pero arriba aparecen objetos virtuales que representan cosas molestas del trabajo. Los agarrás y les pegás con tus propias manos, los hacés rebotar y romperse entre ellos. Es para descargar estrés, pero además tiene habilidad y competencia: cuanto mejor aprendés cómo se comportan los objetos, mejores combinaciones podés hacer.”**

Eso es **SLAPGROUND**.

Todo lo demás que estuvimos discutiendo —C#, Unity, OpenXR, hand tracking, detección del escritorio, física, scores— es **cómo conseguir que exista**.

Y algo importante: todavía lo estamos tratando como **una hipótesis candidata para Meta**, no como “esta es definitivamente la idea que vamos a presentar”.
Development	
Project Setup Tool
Meta XR SDK Documentation
Building Blocks
Hello VR (Samples)
Meta XR Plugin SDKs Documentation
Sample Projects
Project templates & tooling
Working in 3D with Three.js
Loading External Assets
Environment & Lighting
Interactions
Template | Sample Projects
Building for Hands & Eyes	
Interaction SDK Overview
Microgestures | Custom Hand Poses | Grab Interactions
Interaction SDK Samples
Interaction SDK Overview | Microgestures
Integration SDK
Interaction SDK Sample | Hand Pose Showcase
—	
Hands
Built-in Interactions | Locomotion & Microgestures
Testing & Debugging	
Meta XR Simulator
Immersive Debugger
MR Utility Kit
Meta Quest Link (Windows)
Meta XR Simulator
Iteration Speed Best Practices
MR Utility Kit
Meta Spatial Simulator
Meta Horizon Android Studio Plugin
Testing Your Experience
Immersive Web Emulator
Web Launch Test Setup
Performance Optimization	
Meta VR CLI performance tooling
Runtime Optimizer
OVR Metrics Tool
Meta VR CLI performance tooling
Unreal Insights
OVR Metrics Tool
Meta VR CLI performance tooling
Performance Optimizations for WebXR
Build, Deploy & Upload	
Build Configuration Overview
Upload Build to Release Channel
Building and Packaging
App configuration
App Upload Overview
App Upload Overview
Build & Deploy Guide
Deploy via GitHub Pages
Deploy via Vercel
Agentic AI Tooling	
Unity CLI installer
Meta XR Operator
Agentic Tools (Skills)
Agentic Tools (Skills)
Agentic Tools (Skills)
Agentic Tools (Skills)

