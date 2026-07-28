<!-- SEED: re-run /impeccable document once there's code to capture the actual tokens and components. -->

---
name: X-Libra Catering
description: Sistema de gestion de catering profesional
colors:
  primary: "#1a7a8a"
  neutral-bg: "#ffffff"
  surface: "#f5f7fa"
  ink: "#1a1d23"
  muted: "#6b7280"
  accent: "#5b6abf"
  success: "#059669"
  warning: "#d97706"
  danger: "#dc2626"
typography:
  display:
    fontFamily: "Inter, system-ui, -apple-system, sans-serif"
    fontSize: "clamp(1.5rem, 3vw, 2rem)"
    fontWeight: 600
    lineHeight: 1.25
  body:
    fontFamily: "Inter, system-ui, -apple-system, sans-serif"
    fontSize: "0.875rem"
    fontWeight: 400
    lineHeight: 1.5
  label:
    fontFamily: "Inter, system-ui, -apple-system, sans-serif"
    fontSize: "0.8125rem"
    fontWeight: 500
    lineHeight: 1.25
rounded:
  sm: "4px"
  md: "6px"
  lg: "8px"
spacing:
  xs: "4px"
  sm: "8px"
  md: "16px"
  lg: "24px"
  xl: "32px"
components:
  button-primary:
    backgroundColor: "{colors.primary}"
    textColor: "#ffffff"
    rounded: "{rounded.md}"
    padding: "8px 20px"
  button-primary-hover:
    backgroundColor: "#156673"
    textColor: "#ffffff"
    rounded: "{rounded.md}"
    padding: "8px 20px"
  button-secondary:
    backgroundColor: "transparent"
    textColor: "{colors.ink}"
    rounded: "{rounded.md}"
    padding: "8px 20px"
  button-danger:
    backgroundColor: "{colors.danger}"
    textColor: "#ffffff"
    rounded: "{rounded.md}"
    padding: "8px 20px"
  input:
    backgroundColor: "{colors.neutral-bg}"
    textColor: "{colors.ink}"
    rounded: "{rounded.md}"
    padding: "8px 12px"
  card:
    backgroundColor: "{colors.neutral-bg}"
    rounded: "{rounded.lg}"
    padding: "20px"
  table-header:
    backgroundColor: "{colors.surface}"
    textColor: "{colors.muted}"
    rounded: "{rounded.sm}"
    padding: "10px 16px"
  table-cell:
    backgroundColor: "transparent"
    textColor: "{colors.ink}"
    padding: "10px 16px"
  nav-item:
    textColor: "{colors.muted}"
    rounded: "{rounded.md}"
    padding: "8px 16px"
  nav-item-active:
    backgroundColor: "{colors.surface}"
    textColor: "{colors.primary}"
    rounded: "{rounded.md}"
    padding: "8px 16px"
---

# Design System: X-Libra Catering

## 1. Overview

**Creative North Star: "The Quiet Professional"**

A professional catering management system that feels like a well-run kitchen — calm, organized, and quietly precise. Every element is purposeful, every interaction predictable. The design recedes so the data is legible at a glance.

This is a product UI (admin dashboard) for administrative staff managing clients, events, menus, vehicles, and orders. The density is moderate — more information-dense than a marketing site, but with generous whitespace separating logical groups. The interface earns trust through consistency, not flash.

**What this system explicitly rejects:** The AI-generated SaaS template — cream backgrounds, purple accents, glass cards, gradient text, tiny uppercase eyebrows above every section. No restaurant POS boldness (Toast, Clover). No enterprise gray-overload (SAP, Oracle).

**Key Characteristics:**
- Clean, airy layout with deliberate whitespace
- Cool blue-teal anchor for authority and calm
- Fully restrained color strategy — one accent color used on ≤10% of any screen
- Single sans-serif typeface (Inter) for visual simplicity
- Flat by default; minimal elevation for interactive elements only
- Responsive table-based layouts for data density, card-based for detail views

## 2. Colors

A restrained palette anchored by a calm blue-teal. The background is pure white; warmth and personality come from the primary accent, not the surface.

### Primary
- **Calm Teal** (oklch(0.50 0.13 220) / `#1a7a8a`): Primary buttons, active nav state, link text, key interactive elements. A quiet anchor that signals reliability.

### Neutral
- **Pure White** (oklch(1.000 0 0) / `#ffffff`): Page background. Absolute white, no tint.
- **Cool Surface** (oklch(0.965 0.003 220) / `#f5f7fa`): Card backgrounds, table header rows, sidebar. Barely-there cool tint.
- **Dark Ink** (oklch(0.13 0.008 240) / `#1a1d23`): Body text, primary headings. Near-black with a whisper of cool.
- **Muted Silver** (oklch(0.45 0.005 240) / `#6b7280`): Secondary text, placeholder text, table header labels, metadata.

### Semantic
- **Subtle Indigo** (oklch(0.55 0.11 280) / `#5b6abf`): Accent for "information" badges or secondary highlights. Used extremely sparingly.
- **Success Green** (oklch(0.55 0.15 160) / `#059669`): Positive states — "Entregado", "Disponible", confirmation badges.
- **Warning Amber** (oklch(0.65 0.15 80) / `#d97706`): Pending states — "Pendiente", "En Preparacion".
- **Danger Red** (oklch(0.50 0.20 30) / `#dc2626`): Destructive actions, error states, "Eliminar" buttons.

### Named Rules
**The Single Voice Rule.** The Calm Teal primary appears on ≤10% of any given screen — buttons, active nav, and links only. Its rarity is the point. Never use it for decorative backgrounds, borders, or fills. When teal appears, the user knows it is actionable.

## 3. Typography

**Body Font:** Inter, system-ui, -apple-system, sans-serif

**Character:** Inter brings a professional, slightly technical clarity that suits a data-oriented admin tool. It is neutral without being cold, readable at small sizes without being dull. The single-family approach eliminates pairing decisions and keeps the interface visually quiet.

### Hierarchy
- **Display** (Semibold 600, clamp(1.5rem, 3vw, 2rem), 1.25): Page titles (h1). Appears at the top of list and form pages.
- **Headline** (Semibold 600, 1.25rem, 1.3): Section titles within a page (h2-h3). Used for card headings and modal titles.
- **Title** (Medium 500, 1rem, 1.4): Card titles, sidebar nav items.
- **Body** (Regular 400, 0.875rem, 1.5): Table cells, form labels, paragraph text. Max line length 75ch for detail views.
- **Label** (Medium 500, 0.8125rem, 1.25): Form labels, table headers, button text, small metadata. Slightly heavier weight for legibility at small size.
- **Caption** (Regular 400, 0.75rem, 1.3): Helper text, timestamps, secondary metadata.

### Named Rules
**The Single Family Rule.** No font pairing. Inter at varying weights carries both display and body roles. If a heading needs distinction, use weight (600) and size, not a different family.

## 4. Elevation

Flat by default. The interface relies on tonal layering (white surface on cool surface) rather than shadows to indicate hierarchy. Shadows appear only as a response to interaction:

- **Card elevation:** None at rest. Cards are distinguished by background (white on cool surface) and border-radius.
- **Interactive elevation:** Buttons do not lift on hover. Hover is expressed via background darkening (primary buttons) or background tinting (secondary/danger buttons).
- **Modal/Dialog:** A soft ambient shadow (0 8px 32px rgba(0,0,0,0.12)) indicates the dialog is on a separate layer.
- **Dropdowns:** A minimal shadow (0 4px 16px rgba(0,0,0,0.08)) to separate the popover from the page.

### Named Rules
**The Flat-By-Default Rule.** Surfaces are flat at rest. Shadows only appear for dialogs, dropdowns, and tooltips — elements that genuinely exist on a different z-plane.

## 5. Components

### Buttons
- **Shape:** Gently rounded (6px radius). Consistent padding: 8px top/bottom, 20px left/right.
- **Primary (Calm Teal):** `background: #1a7a8a`, `color: white`, `font-weight: 500`, `font-size: 0.875rem`. Hover: darken to `#156673`. Transition: 150ms ease.
- **Secondary (Ghost):** Transparent background, `color: #1a1d23`, 1px solid border `#d1d5db`. Hover: light background tint `#f5f7fa`.
- **Danger (Red):** `background: #dc2626`, `color: white`. Hover: darken. Used for delete actions only.
- **Small variant:** Compact padding (6px 14px), smaller font (0.8125rem). Used inside tables for inline actions.
- **States:** All buttons use `:focus-visible` ring (2px solid `#1a7a8a` offset 2px). No disabled opacity below 0.5.

### Cards / Containers
- **Corner Style:** Rounded (8px radius).
- **Background:** White (`#ffffff`) on the cool surface (`#f5f7fa`) page background.
- **Shadow:** None at rest. Dialog cards use ambient shadow.
- **Border:** 1px solid `oklch(0.92 0.002 240)` for card boundaries, or none if tonal layering is sufficient.
- **Internal Padding:** 20px (spacing-lg). Reduced to 16px for compact card variants.

### Inputs / Fields
- **Style:** 1px solid border `oklch(0.85 0.005 240)`, white background, rounded (6px). Padding 8px 12px.
- **Focus:** Border shifts to Calm Teal (`#1a7a8a`), ring optional (2px `oklch(0.90 0.05 220)`). Transition: 150ms ease.
- **Disabled:** Background `#f9fafb`, text `#9ca3af`, border `#e5e7eb`. No shadow.
- **Error:** Border `#dc2626` with red-tinted ring. Helper text in Danger Red at 0.75rem.
- **Select/Dropdown:** Same styling as text input. Native dropdown arrow preserved for accessibility.

### Tables
- **Header Row:** Background `#f5f7fa`, text `#6b7280` at 0.8125rem Medium 500. Padding 10px 16px. Left-aligned.
- **Body Rows:** White background, alternating "stripes" not used. Text `#1a1d23` at 0.875rem. Padding 10px 16px.
- **Hover:** Row tint `oklch(0.965 0.003 220)` on hover. Transition: 100ms ease.
- **Border:** Clean 1px horizontal borders (`#e5e7eb`) between rows. No vertical borders except for grouping.
- **Responsive:** Horizontal scroll on small screens. Table wrapper with overflow-x: auto.

### Navigation (Sidebar)
- **Style:** Fixed sidebar (250px width), background `#ffffff`. Bottom border on mobile.
- **Items:** Padding 8px 16px, rounded (6px), 0.875rem Medium 500, color `#6b7280`.
- **Active:** Background `#f5f7fa`, color `#1a7a8a`. Left accent bar (3px solid `#1a7a8a`) optional.
- **Hover:** Tint `oklch(0.965 0.003 220)` background.
- **Icon:** 16px inline SVG, `margin-right: 12px`, same color as label. Active state inherits Calm Teal.

### Status Badges
- **Shape:** Rounded (4px). Small inline pill. Padding 2px 10px.
- **Text:** 0.75rem Medium 500, white on filled backgrounds.
- **Color mapping:** Pendiente → Amber, EnPreparacion → Amber, EnRuta → Indigo, Entregado → Green.
- **Boolean states:** Disponible → Green outline badge. No disponible → Gray outline.

## 6. Do's and Don'ts

### Do:
- **Do** use Calm Teal for primary actions only — one CTA per view. Its rarity signals importance.
- **Do** keep backgrounds pure white. The Cool Surface tint is for secondary containers (cards, sidebar) only.
- **Do** use Inter across the entire interface. One family, multiple weights.
- **Do** left-align table content. Numeric columns may right-align (precios, cantidades).
- **Do** use status badges with semantic colors (green for complete, amber for pending, red for danger).
- **Do** keep form labels above inputs, not beside them, for scannability.
- **Do** use `:focus-visible` rings on all interactive elements for keyboard accessibility.
- **Do** use the Flat-By-Default rule: no shadows on cards or surfaces at rest.

### Don't:
- **Don't** use cream backgrounds, purple accents, glass cards, gradient text, or tiny uppercase eyebrows above every section (the AI SaaS template). These are explicitly prohibited.
- **Don't** use the Calm Teal for decorative borders, background fills, or non-interactive elements.
- **Don't** pair Inter with another typeface. The Single Family Rule is absolute.
- **Don't** use border-left or border-right greater than 1px as a colored accent stripe.
- **Don't** use side-stripe borders on cards, callouts, or list items.
- **Don't** use gradient text (`background-clip: text`) anywhere.
- **Don't** use alternating row colors in tables. Clean white rows with subtle hover are sufficient.
- **Don't** animate layout properties. Transitions on color and opacity only.
- **Don't** use glassmorphism or backdrop blur as decoration.
- **Don't** nest cards. One level of containment is the maximum.
