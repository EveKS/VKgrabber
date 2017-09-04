$('#message-contakt').on('submit', function (e) {
    e.stopPropagation();
    e.preventDefault();

    var $form = $(this), url = "/Email/GetEmail/",
        formData = new FormData($form.get(0));

    sendMessage(formData, url, $form)
});

function sendMessage(formData, url, $form) {
    var btn = $form.find('button');
    btn.button('loading');

    $.ajax({
        url: url,
        type: 'post',
        data: formData,
        cache: false,
        dataType: 'json',
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.ok === "ok") {
                var mes = $form.find('#message-submited');
                mes.removeClass('hidden');

                setTimeout(
                    function () {
                        mes.addClass('hidden');
                        $form[0].reset();
                    }, 5000);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus);
        }
    }).always(function () {
        btn.button('reset')
    });
}