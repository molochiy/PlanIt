$('.closeall').click(function () {
    $(this).parents('.panel-heading')
    .next('.panel-collapse')
    .collapse('show')
    .find('.panel-collapse')
    .collapse('toggle');
});