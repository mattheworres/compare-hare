import React from 'react';
import {Page} from '../../layout/components';
import {connect} from 'react-redux';
import {ProductsTable} from './index';
import {Grid} from '@material-ui/core';
import AddProduct1stModal from './create/AddProduct1stModal';

class ProductsDashboard extends React.PureComponent {
  render() {
    return (
      <Page>
        <Grid container spacing={16}>
          <Grid item xs={false} sm={false} md={false} lg={1} />
          <Grid item xs={12} sm={12} md={12} lg={10}>
            <ProductsTable />
          </Grid>
          <Grid item xs={false} sm={false} md={false} lg={1} />
        </Grid>
        <AddProduct1stModal />
      </Page>
    )
  }
}

function mapStateToProps() {
  return {

  };
}

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(ProductsDashboard);
