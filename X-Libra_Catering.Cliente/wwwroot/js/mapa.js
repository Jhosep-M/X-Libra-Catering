window.mapaApp = {
    map: null,
    markers: [],
    marcadorPicker: null,

    _tiles: function () {
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; OpenStreetMap contributors'
        }).addTo(this.map);
    },

    inicializar: function (elementoId, vehiculos) {
        const centro = { lat: -17.7833, lng: -63.1822 };
        this.map = L.map(elementoId).setView(centro, 12);
        this._tiles();
        this.agregarMarcadores(vehiculos);
    },

    agregarMarcadores: function (vehiculos) {
        this.limpiarMarcadores();
        vehiculos.forEach(v => {
            if (v.latitud && v.longitud) {
                const marcador = L.marker([v.latitud, v.longitud])
                    .addTo(this.map)
                    .bindPopup(`
                        <b>${v.marca} ${v.modelo}</b><br/>
                        Placa: ${v.placa}<br/>
                        ${v.direccion ? 'Direcci&oacute;n: ' + v.direccion + '<br/>' : ''}
                        Capacidad: ${v.capacidadKg} kg<br/>
                        Refrigeraci&oacute;n: ${v.tieneRefrigeracion ? 'S&iacute;' : 'No'}<br/>
                        Estado: ${v.disponible ? 'Disponible' : 'Ocupado'}
                    `);
                this.markers.push(marcador);
            }
        });
        if (this.markers.length > 0) {
            const grupo = L.featureGroup(this.markers);
            this.map.fitBounds(grupo.getBounds().pad(0.1));
        }
    },

    limpiarMarcadores: function () {
        this.markers.forEach(m => this.map.removeLayer(m));
        this.markers = [];
    },

    iniciarPicker: function (elementoId, lat, lng) {
        if (!this.map) {
            const centro = (lat && lng) ? { lat: lat, lng: lng } : { lat: -17.7833, lng: -63.1822 };
            this.map = L.map(elementoId).setView(centro, 15);
            this._tiles();
        }
        if (lat && lng) {
            this.marcadorPicker = L.marker([lat, lng], { draggable: true }).addTo(this.map);
        }
        this.map.on('click', (e) => {
            this._colocarPicker(e.latlng.lat, e.latlng.lng);
        });
        if (this.marcadorPicker) {
            this.marcadorPicker.on('dragend', () => {
                const pos = this.marcadorPicker.getLatLng();
                this._ultimaPos = pos;
            });
        }
    },

    _colocarPicker: function (lat, lng) {
        this._ultimaPos = { lat: lat, lng: lng };
        if (this.marcadorPicker) {
            this.marcadorPicker.setLatLng([lat, lng]);
        } else {
            this.marcadorPicker = L.marker([lat, lng], { draggable: true }).addTo(this.map);
            this.marcadorPicker.on('dragend', () => {
                const pos = this.marcadorPicker.getLatLng();
                this._ultimaPos = { lat: pos.lat, lng: pos.lng };
            });
        }
        this.map.setView([lat, lng], 15);
    },

    obtenerPosicionPicker: function () {
        return this._ultimaPos || null;
    },

    geocodificar: function (direccion) {
        const url = 'https://nominatim.openstreetmap.org/search?format=json&q=' + encodeURIComponent(direccion) + '&limit=1';
        return fetch(url, { headers: { 'Accept-Language': 'es' } })
            .then(r => r.json())
            .then(data => {
                if (data && data.length > 0) {
                    return { lat: parseFloat(data[0].lat), lng: parseFloat(data[0].lon), direccion: data[0].display_name };
                }
                return null;
            });
    }
};
