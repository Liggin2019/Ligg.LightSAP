
function showTime() {
    leftSecond -= 1,
        $('#left-sec').text('(' + leftSecond + 's)'),
        0 == leftSecond ? ($(VERIFICATION_PHONE_ID).toggleClass('disabled'), $('#left-sec').text(''), leftSecond = 60) : setTimeout('showTime()', 1000)
}

function goToTop() {
    window.scrollTo(0, 0);
    //alert(window.scrollTop); //undefined
    if (document.body.scrollTop > 0)
        document.body.scrollTop = 0;
}



function renderElement(url, eid) {
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        success: function (result) {
            $("#" + eid).html(result);
        },
    });
}

function updateElementHtml(url, eid, opt) {
    $.ajax({
        type: 'get',
        url: url,
        async: false,
        success: function (rst) {
            if (rst.Flag == 1) {
                var data = rst.Data;
                var html = '';
                if (opt) {
                    if (opt.byName)
                        html = data.Name;
                    else if (opt.byDesc)
                        html = data.Description;
                    else if(data.Body) html = data.Body;
                }
                else if(data.Body) html = data.Body;
                $('#' + eid).html(html);
            }
            else {
                ys.msgError(rst.Message);
            }
        }
    });
}


function setHeaderBackground(transparent) {
    var elmt = document.getElementById("layout-header");
    if (elmt)
        if (transparent) {
            if (elmt.className != 'layout-header transparent-background')
                elmt.className = "layout-header transparent-background"
        }
        else {
            if (elmt.className != 'layout-header header-background')
                elmt.className = "layout-header header-background";
        }
}






