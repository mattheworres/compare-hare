import generateIndexFiles from './generateIndexFile';

generateIndexFiles({
  searchPath: '../app/features',
  includeTests: [/features(?:\/|\\).+?(?:\/|\\)models$/],
  formatInput: fileName => `{default as ${fileName}}`,
});
