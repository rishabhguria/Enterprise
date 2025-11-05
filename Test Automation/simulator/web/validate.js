
// Remove leading whitespace from a string
function trimLeading(s) {
  return s.replace( /^\s+/g, "" );
}

// Remove trailing whitespace from a string
function trimTrailing(s) {
  return s.replace( /\s+$/g, "" );
}

// Trim leading and trailing whitespace from a string
function trim(s) {
  return trimTrailing(trimLeading(s));
}


function isDate(s) {
  if (isEmpty(s)) {
    return true;
  }

  //Format must be YYYYMMDD - ie exactly 8 digits
  if (s.length != 8) {
    return false;
  }

  if (!isNumber(s)) {
    return false;
  }

  return true;
}

function isTime(s) {
  if (isEmpty(s)) {
    return true;
  }

  //Format must be HH:MM:SS - ie exactly 8 characters or
  //YYYYMMDD-HH:MM:SS ie exactly 17 characters.
  if (s.length != 8 && s.length != 17) {
    return false;
  }

  if ( s.length == 8 &&
       (s.charAt(2) != ':' ||
        s.charAt(5) != ':') ) {
    return false;
  }

  if ( s.length == 17 &&
       (s.charAt(8) != '-' ||
        s.charAt(11) != ':' ||
        s.charAt(14) != ':') ) {
    return false;
  }

  return true;
}

function isEmpty(s) {
  if (s == null || s.length == 0) {
    return true;
  }
  return false;

}

function isIoiSize(s) {
  if (isNumber(s)) {
    return true;
  }

  if (s.length > 1) {
    return false;
  }

  c = s.charAt(0);
  if (c == 'S' || c == 'M' || c == 'L') {
    return true;
  }

  return false;
}

function isNumber(s) {
  if (isEmpty(s)) {
    return true;
  }

  var re = /^[-]?\d*\.?\d*$/;
  var ss = s.toString();
  if (!ss.match(re)) {
    return false;
  }
  return true;
}

function isSecurityIDSource(s) {
  s = trimTrailing(s);

  if (isEmpty(s)) {
    return true;
  }

  if (s.length == 1) {
    var re = /^[123456789ABCDEFG]$/;
    if(!s.match(re)) {
      return false;
    }
  } else {
    if (isNumber(s)) {
      var c = parseInt(s);
      if(isNaN(c) || c < 100) {
        return false;
      }
    } else {
      return false;
    }
  }

  return true;
}


/*
  Returns true if all rules pass.
  A rule is an array of triplets in the form:
     <field id> <rule name> <failure message>
*/
function validate( rules ) {

  for (i=0; i < rules.length; i++) {
    field = document.getElementById(rules[i][0]);

    check = rules[i][1];

    mess = rules[i][2];

    switch (check) {

      case 'date':
        if (!isDate(field.value)) {
          field.focus();
          alert( mess );
          return false;
        }
        break;

      case 'time':
        if (!isTime(field.value)) {
          field.focus();
          alert( mess );
          return false;
        }
        break;

      case 'selected':
        //If there is no such field, accept that.
        //This allows us to have optional select fields with a default value
        //provided by a hidden non select field.
        if (field != null && field.selectedIndex == 0) {
          field.focus();
          alert( mess );
          return false;
        }
        break;

      case 'notempty':
        if (isEmpty(field.value)) {
          field.focus();
          alert( mess );
          return false;
        }
        break;

      case 'ioisize':
        if (!isIoiSize(field.value)) {
          field.focus();
          alert( mess );
          return false;
        }
        break;

      case 'number':
        if (!isNumber(field.value)) {
          field.focus();
          alert( mess );
          return false;
        }
        break;

      case 'securityIDSource':
        if (!isSecurityIDSource(field.value)) {
          field.focus();
          alert( mess );
          return false;
        }
        break;
    }
  }

  return true;
}

// Check that
function validateSecurityID() {
  securityIdField = document.getElementById(48);
  securityIdSourceField = document.getElementById(22);

  if (isEmpty(securityIdField.value) != isEmpty(securityIdSourceField.value)) {
    alert( "If you enter a SecurityID then you must enter a SecurityIDSource.  Otherwise both SecurityID and SecurityIDSource must be empty" );
    return false;
  }

  return true;
}