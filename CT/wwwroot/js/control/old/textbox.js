

/*
 * Grid
 * */
class TextBox extends BaseControl {
    //Tên entity bind vào (để build cột)
    EntityName = null

    store = null

    lstColumn = null
    url = null
    height = null

    /*
     * đặt enable/disable
     * phamkhanhhand
     * */
    setEnable(enable) {
        var me = this;

        $('#' + me.htmlID + ' > input').prop("disabled", !enable);
    }

    initControl() {
        let me = this;
        
        $('#' + me.htmlID + '  input').on('focusout', function (event) {
             
            me.on_changeValue(event);
        });

    }

    showRequire(isRequire) {
        let me = this;

        if (isRequire) {

            $('#' + me.htmlID + '  .ct-control-frame').addClass("ct-err-require");
            $('#' + me.htmlID + '  .ct-err-require-all').addClass("require-on");
        }
        else {

            $('#' + me.htmlID + '  .ct-control-frame').removeClass("ct-err-require");
            $('#' + me.htmlID + '  .ct-err-require-all').removeClass("require-on");
        }
    }
     
    on_changeValue(even) {
        let me = this,
            newValue = (event.target && event.target.value) ? event.target.value : null;

        //Check nếu không cho empty
        if (me.ct_require) {

            let rq = true;

            if (NSP.commonFn.isEmpty(newValue)) {
                rq = true;
            }
            else {
                rq = false;
            }

            me.showRequire(rq);
        }
    }

    getValue() {
        let me = this;

        return $('#' + me.htmlID + '  [setfield]').value;
    }

}
