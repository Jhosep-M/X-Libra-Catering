### Task 1: Estilos CSS compartidos para las 4 páginas

**Files:**
- Modify: `X-Libra_Catering.Cliente/wwwroot/css/app.css` (agregar al final)

**Interfaces:**
- Consumes: Variables CSS existentes (`--color-primary`, `--radius-lg`, etc.)
- Produces: Clases CSS que las 4 páginas usarán (`.page-header`, `.card-grid`, .`contact-card`, `.badge-categoria-*`, `.status-badge`, `.kpi-strip`, `.pedido-kanban`)

- [ ] **Step 1: Agregar estilos base de page header y toolbar**

```css
/* app.css — al final del archivo */

/* ---- Shared Page Patterns ---- */

.page-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  margin-bottom: var(--space-lg);
}

.page-header h3 {
  margin-bottom: 4px;
}

.page-header p {
  margin: 0;
  font-size: var(--font-size-sm);
  color: var(--color-muted);
}

.toolbar {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: var(--space-lg);
  flex-wrap: wrap;
}

.toolbar .search-box {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  padding: 8px 12px;
  flex: 1;
  min-width: 200px;
  max-width: 360px;
  transition: border-color var(--ease-fast);
}

.toolbar .search-box:focus-within {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 2px var(--color-primary-light);
}

.toolbar .search-box svg {
  color: var(--color-muted);
  flex-shrink: 0;
  width: 16px;
  height: 16px;
}

.toolbar .search-input {
  border: none;
  outline: none;
  background: transparent;
  font-family: var(--font-sans);
  font-size: var(--font-size-base);
  color: var(--color-ink);
  width: 100%;
}

.toolbar .filter-select {
  font-family: var(--font-sans);
  font-size: var(--font-size-sm);
  padding: 8px 12px;
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  background: var(--color-bg);
  color: var(--color-ink);
  min-width: 160px;
  cursor: pointer;
}

.toolbar .filter-select:focus {
  border-color: var(--color-primary);
  outline: none;
}

.result-count {
  font-size: var(--font-size-sm);
  color: var(--color-muted);
  margin-left: auto;
  white-space: nowrap;
}

.empty-state {
  text-align: center;
  padding: 60px 20px;
  color: var(--color-muted);
}

.empty-state svg {
  width: 64px;
  height: 64px;
  margin-bottom: var(--space-md);
  opacity: 0.4;
}

.empty-state h4 {
  font-size: var(--font-size-lg);
  margin-bottom: var(--space-sm);
  color: var(--color-ink);
}

.empty-state p {
  font-size: var(--font-size-sm);
}
```

- [ ] **Step 2: Agregar estilos de card grid y contact-card**

```css
/* ---- Card Grid (shared) ---- */

.card-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: var(--space-md);
}

@media (max-width: 1024px) {
  .card-grid { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .card-grid { grid-template-columns: 1fr; }
}

/* ---- Contact Card (Clientes) ---- */

.contact-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 20px;
  display: flex;
  align-items: flex-start;
  gap: 16px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
}

.contact-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.contact-avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background: var(--color-primary-light);
  color: var(--color-primary);
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: var(--font-size-lg);
  flex-shrink: 0;
}

.contact-body {
  flex: 1;
  min-width: 0;
}

.contact-body h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin-bottom: 6px;
  color: var(--color-ink);
}

.contact-info {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 16px;
}

.contact-info .info-item {
  display: inline-flex;
  align-items: center;
  gap: 4px;
  font-size: var(--font-size-xs);
  color: var(--color-muted);
}

.contact-info .info-item svg {
  width: 14px;
  height: 14px;
  opacity: 0.6;
  flex-shrink: 0;
}

.card-footer-actions {
  display: flex;
  justify-content: flex-end;
  gap: 6px;
  margin-top: 10px;
  padding-top: 10px;
  border-top: 1px solid var(--color-border-light, #f0f0f0);
  opacity: 0;
  transition: opacity var(--ease-fast);
}

.contact-card:hover .card-footer-actions,
.menu-card:hover .card-footer-actions,
.vehicle-card:hover .card-footer-actions {
  opacity: 1;
}

.btn-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 28px;
  height: 28px;
  border-radius: var(--radius-sm);
  border: none;
  background: transparent;
  color: var(--color-muted);
  cursor: pointer;
  transition: all var(--ease-fast);
  padding: 0;
}

.btn-icon-edit:hover {
  background: var(--color-primary-light);
  color: var(--color-primary);
}

.btn-icon-delete:hover {
  background: var(--color-danger-bg, #fef2f2);
  color: var(--color-danger);
}
```

- [ ] **Step 3: Agregar estilos de menu-card (catálogo)**

```css
/* ---- Menu Card (Menús) ---- */

.menu-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 20px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
}

.menu-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.menu-icon {
  font-size: 2rem;
  margin-bottom: 8px;
}

.menu-card h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin-bottom: 4px;
  color: var(--color-ink);
}

.menu-desc {
  font-size: var(--font-size-xs);
  color: var(--color-muted);
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  line-height: 1.4;
  margin-bottom: 10px;
  min-height: 2.8em;
}

.menu-meta {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 8px;
  margin-bottom: 10px;
}

.menu-price {
  font-size: var(--font-size-lg);
  font-weight: 700;
  color: var(--color-primary);
}

.badge-categoria {
  display: inline-flex;
  align-items: center;
  font-size: var(--font-size-xs);
  font-weight: 500;
  padding: 2px 8px;
  border-radius: 4px;
}

.badge-categoria.entrada { background: #e0f2fe; color: #0369a1; }
.badge-categoria.plato-fuerte { background: #ccfbf1; color: #0f766e; }
.badge-categoria.postre { background: #fef3c7; color: #b45309; }
.badge-categoria.bebida { background: #ede9fe; color: #6d28d9; }
```

- [ ] **Step 4: Agregar estilos de vehículo y pedidos**

```css
/* ---- KPI Strip (Vehículos) ---- */

.kpi-strip {
  display: flex;
  gap: var(--space-md);
  margin-bottom: var(--space-lg);
  flex-wrap: wrap;
}

.kpi-strip-item {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  padding: 12px 16px;
  font-size: var(--font-size-sm);
  font-weight: 500;
  color: var(--color-muted);
}

.kpi-strip-item strong {
  font-size: var(--font-size-lg);
  color: var(--color-ink);
}

.kpi-strip-item .kpi-dot {
  width: 8px;
  height: 8px;
  border-radius: 50%;
}

.kpi-dot.green { background: var(--color-success); }
.kpi-dot.red { background: var(--color-danger); }

/* ---- Vehicle Card ---- */

.vehicle-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 20px;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
}

.vehicle-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.vehicle-top {
  display: flex;
  align-items: center;
  gap: 12px;
  margin-bottom: 10px;
}

.vehicle-icon {
  font-size: 1.75rem;
}

.vehicle-top h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin: 0;
  color: var(--color-ink);
}

.vehicle-plate {
  font-family: 'Courier New', monospace;
  font-size: var(--font-size-sm);
  font-weight: 600;
  background: var(--color-surface);
  padding: 2px 8px;
  border-radius: var(--radius-sm);
  color: var(--color-ink);
  letter-spacing: 0.05em;
}

.vehicle-details {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 16px;
  margin-bottom: 10px;
}

.vehicle-details .detail-item {
  font-size: var(--font-size-xs);
  color: var(--color-muted);
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.vehicle-status {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  font-size: var(--font-size-sm);
  font-weight: 600;
  padding: 4px 12px;
  border-radius: 20px;
}

.vehicle-status.disponible {
  background: var(--color-success-bg);
  color: var(--color-success);
}

.vehicle-status.ocupado {
  background: var(--color-danger-bg, #fef2f2);
  color: var(--color-danger);
}

/* ---- Pedido Kanban ---- */

.pedido-kanban {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: var(--space-md);
  min-height: 400px;
}

.pedido-kanban .kanban-column {
  background: var(--color-surface);
  border-radius: var(--radius-lg);
  display: flex;
  flex-direction: column;
  min-height: 300px;
}

.pedido-kanban .column-header {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 16px 16px 12px;
  border-bottom: 1px solid var(--color-border);
}

.pedido-kanban .column-header h4 {
  font-size: var(--font-size-sm);
  font-weight: 600;
  margin: 0;
  flex: 1;
  text-transform: uppercase;
  letter-spacing: 0.03em;
}

.pedido-kanban .column-body {
  padding: 12px;
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 10px;
  overflow-y: auto;
}

.pedido-card {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-lg);
  padding: 14px;
  cursor: grab;
  transition: box-shadow var(--ease-fast), transform var(--ease-fast);
  user-select: none;
}

.pedido-card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
  transform: translateY(-1px);
}

.pedido-card h5 {
  font-size: var(--font-size-base);
  font-weight: 600;
  margin-bottom: 6px;
  color: var(--color-ink);
}

.pedido-card .pedido-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 4px 14px;
  margin-bottom: 4px;
}

.pedido-card .pedido-meta span {
  font-size: var(--font-size-xs);
  color: var(--color-muted);
  display: inline-flex;
  align-items: center;
  gap: 4px;
}

.pedido-card .pedido-total {
  font-weight: 700;
  color: var(--color-primary);
  font-size: var(--font-size-base);
  margin-top: 4px;
}

@media (max-width: 1024px) {
  .pedido-kanban { grid-template-columns: repeat(2, 1fr); }
}

@media (max-width: 640px) {
  .pedido-kanban { grid-template-columns: 1fr; }
}
```
