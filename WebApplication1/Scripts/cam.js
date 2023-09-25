function take_snapshot() {
    // take snapshot and get image data
    Webcam.snap(function (data_uri) {
        // display results in page
        document.getElementById('results').innerHTML =
            '<h2>Here is your image:</h2>' +
            '<img id="base64image" src="' + data_uri + '"/>';
    });
}


