import {List} from 'immutable';

export default function(models, ModelRecord) {
  const result = List().asMutable();

  for (const model of models) {
    result.push(new ModelRecord(model));
  }

  return result.asImmutable();
}
