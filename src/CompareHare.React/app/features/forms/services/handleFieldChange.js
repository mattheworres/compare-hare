export default function(event, data, setFormField) {
  const {className, type, name, checked, value, radio, action} = data;

  if (data && className === 'select') {
    setFormField(name, value, action);
  } else if (radio === true) {
    event.preventDefault();
    setFormField(name, value, action);
  } else if (type === 'checkbox') {
    setFormField(name, checked, action);
  } else {
    setFormField(name, value, action);
  }
}
