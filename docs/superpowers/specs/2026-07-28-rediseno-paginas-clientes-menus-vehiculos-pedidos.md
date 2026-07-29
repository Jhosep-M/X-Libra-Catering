# Rediseño de Páginas: Clientes, Menús, Vehículos, Pedidos

## Objetivo
Reemplazar las tablas HTML actuales en las páginas de Clientes, Menús, Vehículos y Pedidos por interfaces visuales con personalidad propia, siguiendo el diseño "Quiet Professional" del proyecto (teal `#0d9488`, Inter, cards con borde, sombras suaves).

## Alcance
Solo frontend (archivos `.razor` + CSS en `app.css`). No se modifican DTOs, servicios, controladores ni modelos.

---

## 1. Clientes — "Tarjetero / Directorio"

**Layout:** Grid de cards responsive (3 columnas → 2 → 1).

**Cada card contiene:**
- Avatar circular con iniciales (letra capital del nombre, fondo teal claro, texto teal)
- Nombre del cliente en semibold
- Teléfono con ícono 📞
- Email con ícono ✉
- Dirección con ícono 📍 (truncada si es muy larga)
- Botones: Editar (ícono lápiz) y Eliminar (ícono papelera) — aparecen al hover

**Toolbar:**
- Input de búsqueda (busca en nombre, teléfono, email)
- Botón "+ Agregar Cliente"

**Estados vacío:** Mensaje "No hay clientes registrados" con ilustración minimalista.

---

## 2. Menús — "Catálogo"

**Layout:** Grid de cards responsive (3 columnas → 2 → 1).

**Cada card contiene:**
- Icono representativo (🍽 para comida, 🥗 ensaladas, 🍰 postres, etc. según categoría)
- Nombre del menú (semibold)
- Descripción corta (2 líneas max, truncada con ellipsis)
- Badge de categoría con color:
  - Entrada → azul
  - Plato Fuerte → teal
  - Postre → amarillo
  - Bebida → violeta
- Precio formateado (moneda) destacado
- Indicador "🧊 Refrigeración: Sí/No"
- Botones Editar / Eliminar al hover

**Toolbar:**
- Input de búsqueda por nombre
- Select de filtro por categoría
- Botón "+ Agregar Menú"

---

## 3. Vehículos — "Flota / Garage"

**Layout:** Grid de cards (3 columnas → 2 → 1) con un mini KPI arriba.

**Mini KPI:** Una fila horizontal con:
- "🚛 Total: N"
- "● Disponibles: N" (verde)
- "● Ocupados: N" (rojo/ámbar)

**Cada card contiene:**
- Icono de vehículo (🚚 furgoneta, 🚐 van, 🚛 camión según tipo o general)
- Marca y Modelo
- Placa destacada (monospace)
- Badge de capacidad (ej. "1500 kg")
- Indicador 🧊 Refrigeración: Sí/No
- Indicador de estado grande y colorido:
  - Disponible → badge verde "● Disponible"
  - Ocupado → badge rojo/ámbar "● Ocupado"
- Botones Editar / Eliminar al hover

**Toolbar:**
- Input de búsqueda (marca, modelo, placa)
- Select de filtro: "Todos", "Disponibles", "Ocupados"
- Botón "+ Agregar Vehículo"

---

## 4. Pedidos — "Kanban de Órdenes"

**Layout:** Tablero Kanban con 4 columnas (misma mecánica que EventosDashboard):
- Pendientes
- En Preparación
- En Ruta
- Entregados

**Cada card contiene:**
- Número de pedido (ej. #104) semibold
- Nombre del evento asociado
- Vehículo asignado (o "🚚 —" si no asignado)
- Fecha del pedido
- Total formateado

**Toolbar:**
- Input de búsqueda por ID o nombre de evento
- Botón "+ Agregar Pedido"

**Drag & drop** entre columnas (HTML5, mismo patrón que EventosDashboard) con botones de acción rápida como respaldo.

---

## Diseño y UX

- **Avatar iniciales:** div circular de 40px, fondo `var(--color-primary-light)`, texto `var(--color-primary)`, font-weight 600.
- **Cards:** `border-radius: var(--radius-lg)`, borde `var(--color-border)`, sombra sutil, transición hover.
- **Badges:** mismos estilos pastel usados en EventosDashboard.
- **Toolbar:** mismo patrón (search box + filter select + botón primario).
- **Responsive:** mismo breakpoint system (tablet 2 cols, mobile 1 col + toolbar vertical).
- **Sin framework externo:** CSS nativo con variables, sin Bootstrap classes (el diseño actual ya es custom).

---

## Lo que NO cambia

- Navegación (NavMenu sigue igual)
- Formularios de creación/edición (ClienteForm, MenuForm, VehiculoForm, PedidoForm)
- Backend, DTOs, servicios
- Lógica de negocio
- Archivos Shared y Server

## Archivos a modificar

| Archivo | Acción |
|---------|--------|
| `Cliente/Pages/Clientes.razor` | Reescribir |
| `Cliente/Pages/Menus.razor` | Reescribir |
| `Cliente/Pages/Vehiculos.razor` | Reescribir |
| `Cliente/Pages/Pedidos.razor` | Reescribir |
| `Cliente/wwwroot/css/app.css` | Agregar estilos nuevos |
