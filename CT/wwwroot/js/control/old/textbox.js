

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

            $('#' + me.htmlID + '  .mb-control-frame').addClass("mb-err-require");
            $('#' + me.htmlID + '  .mb-err-require-all').addClass("require-on");
        }
        else {

            $('#' + me.htmlID + '  .mb-control-frame').removeClass("mb-err-require");
            $('#' + me.htmlID + '  .mb-err-require-all').removeClass("require-on");
        }
    }
     
    on_changeValue(even) {
        let me = this,
            newValue = (event.target && event.target.value) ? event.target.value : null;

        //Check nếu không cho empty
        if (me.mb_require) {

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
