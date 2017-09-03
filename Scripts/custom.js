$(document).ready(function () {
    
    $('body').on('select', 'DropDownListStatus', function (event) {
        event.preventDefault();
        var retMVal, retTVal, retRVal, optMVal, optRVal, newMultiple, numCheck, values, newCheck, i;
        $('#DropDownListStatus').find('[value=0]');
        values = $('#DropDownListStatus').val();
        if (values === '0') {
            retMVal = prompt('Enter your Question:', 'Your Question Here');
            newMultiple = jQuery('<div class="form-group edit"><label>' + retMVal + '</label><br/></div>');
            jQuery('#new').append(newMultiple);
            var valuesMCQ = $('#specifyMCQ').val();
            for (i = 0; i < valuesMCQ; i++) {
                optMVal = prompt('Enter Option Value', 'Option Value');
                $('#new').append('<input type="checkbox"><span class="edit" data-showbuttons="true" data-mode="inline" data-placement="right"> ' + optMVal + '</span><br/>');
}
            jQuery('#new').append('<hr/>');
} else if (values === '1') {
            retTVal = prompt('Enter your Question:', 'Your Question Here');
            var newText = jQuery('<div class="form-group"><label>' + retTVal + '</label><br/><input type="text" class="form-control" placeholder="Enter Answer Here"></div><hr/>');
            jQuery('#new').append(newText);
} else if (values === '2') {
            retRVal = prompt('Enter your Question:', 'Your Question Here');
            var newRadio = jQuery('<div class="form-group edit"><label>' + retRVal + '</label> <br/></div>');
            jQuery('#new').append(newRadio);
            var valuesRadio = $('#specifyRadio').val();
            for (i=0; i < valuesRadio; i++) {
                optRVal = prompt('Enter Option Value', 'Option Value');
                $('#new').append('<span><input type="radio" class="edit" name="optionsRadios" id="optionsRadios1" value="option"> ' + optRVal + '</span><br/>');
}
            jQuery('#new').append('<hr/>');
}
        $('.add-question').selectpicker('refresh');
});
});