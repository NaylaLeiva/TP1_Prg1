# Trabajo Práctico 1 - Programación 1

## Descripción del Juego
Juego en primera persona desarrollado en Unity donde el jugador debe navegar por un escenario con plataformas y obstáculos, evitar generadores automáticos de objetos, activar un potenciador de velocidad y transportar una caja desde la zona inicial hasta la zona de entrega para lograr la victoria.

- **Versión de Unity:** Unity 6 (6000.0.25f1)

---

## Controles
- **W, A, S, D:** Mover el personaje.
- **Mouse:** Control de la cámara / Mirar alrededor.
- **Espacio:** Saltar.
- **Shift Izquierdo:** Correr.
- **Tecla E:** Agarrar / Soltar la caja interactiva.
- **Tecla Q:** Activar Power-Up de velocidad con cooldown.

---

## Mecánicas Implementadas

1. **Consigna 1 - Construcción del Escenario y Jugador:**
   * Diseño del nivel con plataformas fijas y móviles.
   * Configuración del personaje con movimiento en 3D, salto y físicas con Rigidbody.

2. **Consigna 2 - Generador Temporal con `Invoke`:**
   * Generación programada de elementos u obstáculos utilizando el método `Invoke` para ejecuciones con retardo diferido.

3. **Consigna 3 - Generador Continuo con `InvokeRepeating`:**
   * Espagado/Spawneo continuo e interactivo de objetos en el mapa mediante `InvokeRepeating` a intervalos regulares de tiempo.

4. **Consigna 4 - Sistema de Interacción / Agarre:** 
   * Detección de objetos interactivos por Raycast y proximidad.
   * Emparentamiento de la caja al personaje sin colisiones de interferencia.
   * Reseteo automático de posición de la caja al caer al vacío o al reaparecer el jugador.

5. **Consigna 5 - Power-Up de Velocidad:**
   * Incremento temporal de velocidad de desplazamiento al presionar la tecla `Q`.
   * Feedback visual mediante cambio de color en el material del personaje (Activo / Cooldown / Normal).

6. **Consigna 6 - Zona de Victoria (Drop Zone):**
   * Detección de la caja mediante un volumen Trigger en la zona final.
   * Muestra mensaje de victoria en pantalla (UI) y cambia el color de la zona al entregar la caja.

---

## Instrucciones para abrir y ejecutar el proyecto
1. Clonar el repositorio: `git clone https://github.com/NaylaLeiva/TP1_Prg1.git`
2. Abrir **Unity Hub**.
3. Hacer clic en **Add** -> **Add project from disk** y seleccionar la carpeta del proyecto.
4. Abrir con **Unity 6 (6000.0.25f1)** o superior.
5. Cargar la escena **`Nivel1`** ubicada en `Assets/Scenes/` y presionar **Play**.

---

## Galería del Proyecto

### Salida
<img width="1917" height="786" alt="image" src="https://github.com/user-attachments/assets/cd3a8df8-46e9-4c72-aefd-71329b1dcc70" />

### Primera Vista
<img width="1917" height="782" alt="image" src="https://github.com/user-attachments/assets/149c70d0-feda-4bf7-bf76-c7525aae3ff8" />

### Vista de Plataformas
<img width="1917" height="771" alt="image" src="https://github.com/user-attachments/assets/47ec637e-79a6-42c5-b566-4c5b3549f36d" />

### Meta
<img width="1917" height="766" alt="image" src="https://github.com/user-attachments/assets/89aec820-685e-431a-b8e5-74c015d0adbf" />

### Mensaje Final
<img width="1917" height="771" alt="image" src="https://github.com/user-attachments/assets/de05f902-1316-4ba0-a8e8-ee20706807f1" />

## Importación y Configuración de Assets Externos

Para la construcción del entorno y el personaje se importaron y configuraron paquetes de assets 3D optimizados desde la Unity Asset Store / Packages externos:

1. **Asset de Personaje (3D Character & Animations):**
   * **Importación:** Configuración e integración del modelo 3D del jugador junto con su esqueleto (Rig) en modo *Generic/Humanoid*.
   * **Componentes:** Integración con `CharacterController` / `Rigidbody`, cámara en primera persona ajustada a la altura de la cabeza (`cameraHolder`) y sistema de `Animator` para la transición de animaciones (Caminar, Correr, Saltar).

2. **Asset de Escenario y Entorno (Environment Assets):**
   * **Importación:** Kit modular de escenario (*Forest/Poly Pack*) que incluye modelos de plataformas, estructuras de puente (`ForestBridgeRegular`), banderas (`ForestFlag_Blue`), cajas decorativas e interactivas (`ForestCrate`), árboles y obstáculos.
   * **Configuración:** Asignación de componentes físicos (`Box Collider`, `Mesh Collider`) e integración de materiales con shaders optimizados para el renderizado del nivel.

3. **Otros Assets y UI (Canvas & TextMeshPro):**
   * **Importación de TextMeshPro:** Paquete esencial de UI importado para la renderización de texto vectorial en alta resolución.
   * **Prefabs e Interactivos:** Configuración de prefabs reutilizables para plataformas móviles, coleccionables y las zonas de entrega (`DropZone01` y `DropZone02`).
