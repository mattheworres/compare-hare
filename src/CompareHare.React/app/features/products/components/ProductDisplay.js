import React from 'react';
import {Page} from '../../layout/components';
import {connect} from 'react-redux';
import {Grid, Tab, Tabs} from '@material-ui/core';
import {AttachMoney, Timeline} from '@material-ui/icons';
import autobind from 'class-autobind';
import ProductCurrentTable from './ProductCurrentTable';
import {loadProductCurrent} from '../actions/productDisplay';
import ProductHistoryTable from './ProductHistoryTable';

const initialState = {
  tab: 0,
  trackedProductId: null
};

class ProductDisplay extends React.PureComponent {
  constructor(props) {
    super(props);

    this.state = initialState;

    autobind(this);
  }

  componentDidMount() {
    const {match, loadProductCurrent} = this.props;
    const {trackedProductId} = match.params;

    this.setState({trackedProductId});

    loadProductCurrent(trackedProductId);
  }

  changeTab(event, value) {
    this.setState({tab: value});
  }
  
  render() {
    const { tab, trackedProductId } = this.state;
    const { loadProductCurrent } = this.props;
    return (
      <Page>
        <Grid container spacing={16}>
          <Grid item xs={false} sm={false} md={false} lg={1} />
          <Grid item xs={12} sm={12} md={12} lg={10}>
            <Tabs value={tab}
              onChange={this.changeTab}
              indicatorColor="secondary"
              textColor="secondary">
                <Tab icon={<AttachMoney />} label="Current" />
                <Tab icon={<Timeline />} label="Historical" />
            </Tabs>
            {tab === 0
              ? <ProductCurrentTable loadProductCurrent={loadProductCurrent} trackedProductId={trackedProductId} />
              : <ProductHistoryTable />}
          </Grid>
          <Grid item xs={false} sm={false} md={false} lg={1} />
        </Grid>
      </Page>
    )
  }
}

function mapStateToProps() {
  return {

  };
}

const mapDispatchToProps = {
  loadProductCurrent
};

export default connect(mapStateToProps, mapDispatchToProps)(ProductDisplay);
