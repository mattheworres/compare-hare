import {List} from 'immutable';

export default function(
  models,
  ModelRecord,
  NestedModelRecord,
  nestedCollectionName,
) {
  const result = List().asMutable();

  for (const model of models) {
    let nestedModels = List().asMutable();

    if (model[nestedCollectionName] !== undefined) {
      for (const nestedModel of model[nestedCollectionName]) {
        nestedModels.push(new NestedModelRecord(nestedModel));
      }

      let newModel = new ModelRecord(model);
      result.push(newModel.set(nestedCollectionName, nestedModels));
    } else {
      result.push(new ModelRecord(model));
    }
  }

  return result.asImmutable();
}
