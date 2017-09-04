$('#goToTop').on('click', function (e) {
    e.stopPropagation();

    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
});
$('#load-content, #get-messages, #add-group-from, #modal-group-option-submit, #btn-save-token').on('click', function () {
    var $this = $(this);
    $this.button('loading');
});

$(function () {
    $('div.thumb.im-prev').on('click', function (e) {
        e.preventDefault();

        $('#image-modal .modal-body img')
            .attr('src', $(this).find('img').attr('src'));

        $("#image-modal").modal('show');
    });

    $('#image-modal .modal-body img').on('click', function () {
        $("#image-modal").modal('hide')
    });
});

$(function () {
    $('.copy_txt').html(email());
});

$('.copy_txt').on('click', 'a', function () {
    var that = $(this);

    that.attr('href', that.attr('href')
        .replace(/GGG/, '@')
        .replace(/RRR/, '.'));
});

function email() {
    var text = '&copy;' + ' Eve' + 'KS<br />';
    var attr = ' class="btn btn-primary center" ';
    var adres = 'eve' + 'ks@vkgraber&#46;<![if !IE]>r<![endif]>u';
    var login = 'eve' + 'ks';
    var server = 'vkgra' + 'berRRRru';
    var request = '?subject=vkma' + 'nager';
    var email = login + 'GGG' + server;
    var url = 'mai' + 'lto:' + email + request;
    var result = text + '<a' + attr + 'href="' + url + '">' + adres + '</a>';
    return result;
}

