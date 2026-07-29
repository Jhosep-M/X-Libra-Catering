function crearGraficoBarras(elementoId, labels, datos, titulo) {
    const ctx = document.getElementById(elementoId);
    if (!ctx) return;
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: titulo || '',
                data: datos,
                backgroundColor: 'rgba(26, 122, 138, 0.7)',
                borderColor: 'rgba(26, 122, 138, 1)',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
}

function crearGraficoDona(elementoId, labels, datos) {
    const ctx = document.getElementById(elementoId);
    if (!ctx) return;
    const colores = [
        'rgba(26, 122, 138, 0.8)',
        'rgba(5, 150, 105, 0.8)',
        'rgba(245, 158, 11, 0.8)',
        'rgba(239, 68, 68, 0.8)',
        'rgba(91, 106, 191, 0.8)',
        'rgba(156, 163, 175, 0.8)'
    ];
    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labels,
            datasets: [{
                data: datos,
                backgroundColor: colores.slice(0, datos.length),
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: { position: 'bottom' }
            }
        }
    });
}
