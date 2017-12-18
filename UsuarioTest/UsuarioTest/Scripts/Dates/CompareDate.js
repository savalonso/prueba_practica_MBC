function compareDates(firstDateId, secondDateId) {
    const firstDay = document.getElementById(firstDateId).value.split("-");
    const secondDay = document.getElementById(secondDateId).value.split("-");
    let day1 = new Date(firstDay[0], firstDay[1] - 1, firstDay[2]);
    let day2 = new Date(secondDay[0], secondDay[1] - 1, secondDay[2]);
    if (day2 > day1) {
        document.getElementById('user_alert').style.display = 'none';
        const tiempomil = day2 - day1;
        const tiemposeg = tiempomil / 1000;
        const tiempomin = tiemposeg / 60;
        const tiempohor = tiempomin / 60;
        const tiempodias = tiempohor / 24;
        document.getElementById('result').style.display = '';
        document.getElementById('result').innerHTML = "La diferencia en días es de: " + tiempodias +
            "</br>La diferencia en minutos es de: " + tiempomin + "</br>La diferencia en segundos es de: " + tiemposeg;
    } else {
        document.getElementById('result').style.display = 'none';
        document.getElementById('user_alert').style.display = '';
    }
}