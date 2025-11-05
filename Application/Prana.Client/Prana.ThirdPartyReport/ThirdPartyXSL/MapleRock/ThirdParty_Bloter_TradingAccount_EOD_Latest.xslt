<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template name="DateFormat">
    <xsl:param name="Date"/>
    <xsl:value-of select="concat(substring-before(substring-after($Date,'-'),'-'),'/',substring-before(substring-after(substring-after($Date,'-'),'-'),'T'),'/',substring-before($Date,'-'))"/>
  </xsl:template>
  <xsl:template match="/NewDataSet">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
      <ThirdPartyFlatFileDetail>

        <RowHeader>
          <xsl:value-of select ="'true'"/>
        </RowHeader>

        <IsCaptionChangeRequired>
          <xsl:value-of select ="'true'"/>
        </IsCaptionChangeRequired>

        <TaxLotState>
          <xsl:value-of select="'TaxLotState'"/>
        </TaxLotState>


        <TradeDate>
          <xsl:value-of select="'Trade Date'"/>
        </TradeDate>


        <SettlementDate>
          <xsl:value-of select="'Settlement Date'"/>
        </SettlementDate>

        <Trader>
          <xsl:value-of select="'Trader'"/>
        </Trader>
        
        <Broker>
          <xsl:value-of select="'Broker'"/>
        </Broker>

        <BuySell>
          <xsl:value-of select="'B/S'"/>
        </BuySell>

        <TickerSymbol>
          <xsl:value-of select="'Ticker Symbol'"/>
        </TickerSymbol>

        <CusipSymbol>
          <xsl:value-of select="'Cusip Symbol'"/>
        </CusipSymbol>

        <SecurityDesc>
          <xsl:value-of select="'Security Description Name'"/>
        </SecurityDesc>

        <TradeQuantity>
          <xsl:value-of select="'Trade Quantity'"/>
        </TradeQuantity>

        <AllocatedQuantity>
          <xsl:value-of select="'Allocated Quantity'"/>
        </AllocatedQuantity>

        <Account>
          <xsl:value-of select="'Allocations'"/>
        </Account>

        <Price>
          <xsl:value-of select="'Price'"/>
        </Price>


        <CmsnPerShareRate>
          <xsl:value-of select="'Commission in cents Per share'"/>
        </CmsnPerShareRate>

        <Commission>
          <xsl:value-of select="'Commission'"/>
        </Commission>

        <CommissioninBase>
          <xsl:value-of select="'Commission in Base'"/>
        </CommissioninBase>
        
        <SECFees>
          <xsl:value-of select="'SEC Fees'"/>
        </SECFees>

        <ORFFees>
          <xsl:value-of select="'ORF Fees'"/>
        </ORFFees>

        <OCCFees>
          <xsl:value-of select="'OCC Fees'"/>
        </OCCFees>

        <AllOtherFees>
          <xsl:value-of select="'All Other Fees'"/>
        </AllOtherFees>

        <AccruedInterest>
          <xsl:value-of select="'Accrued Interest'"/>
        </AccruedInterest>


        <NetAmountLocal>
          <xsl:value-of select="'Net Amount (Local)'"/>
        </NetAmountLocal>


        <FXRate>
          <xsl:value-of select="'FX Rate'"/>
        </FXRate>

        <NetAmountBase>
          <xsl:value-of select="'Net Amount (Base)'"/>
        </NetAmountBase>

        <AssetClass>
          <xsl:value-of select="'AssetClass'"/>
        </AssetClass>

        <BorrowBroker>
          <xsl:value-of select="'BorrowBroker'"/>
        </BorrowBroker>

        <BorrowQty>
          <xsl:value-of select="'BorrowQty'"/>
        </BorrowQty>

        <BorrowID>
          <xsl:value-of select="'BorrowID'"/>
        </BorrowID>


        <BorrowRate>
          <xsl:value-of select="'BorrowRate'"/>
        </BorrowRate>

        <EntityID>
          <xsl:value-of select="'EntityID'"/>
        </EntityID>



      </ThirdPartyFlatFileDetail>
    
      <xsl:for-each select="ThirdPartyFlatFileDetail">
        
        <xsl:variable name="varNetamount">
          <xsl:choose>
            <xsl:when test="contains(Side,'Buy')">
              <xsl:value-of select="(AllocatedQty * AvgPrice * AssetMultiplier) + (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
            <xsl:when test="contains(Side,'Sell')">
              <xsl:value-of select="(AllocatedQty * AvgPrice * AssetMultiplier) - (CommissionCharged + SoftCommissionCharged + OtherBrokerFees + ClearingBrokerFee + StampDuty + TransactionLevy + ClearingFee + TaxOnCommissions + MiscFees + SecFee + OccFee + OrfFee)"/>
            </xsl:when>
          </xsl:choose>
        </xsl:variable>
        <ThirdPartyFlatFileDetail>

          <RowHeader>
            <xsl:value-of select ="'true'"/>
          </RowHeader>

          <IsCaptionChangeRequired>
						<xsl:value-of select ="'true'"/>
					</IsCaptionChangeRequired>

          <TaxLotState>
            <xsl:value-of select="TaxLotState"/>
          </TaxLotState>

         
          <TradeDate>
            <xsl:value-of select="TradeDate"/>
          </TradeDate>

         
          <SettlementDate>
            <xsl:value-of select="SettlementDate"/>
          </SettlementDate>

          <Trader>
            <xsl:value-of select="TradingAccount"/>
          </Trader>

          <Broker>
            <xsl:value-of select="CounterParty"/>

          </Broker>

          <BuySell>
            <xsl:choose>
              <xsl:when test ="Side='Sell short' or Side='Sell to Open'">
                <xsl:value-of select ="'SS'"/>
              </xsl:when>
              <xsl:when test ="Side='Buy to Cover' or Side='Buy to Close'">
                <xsl:value-of select ="'CB'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring(Side,1,1)"/>
              </xsl:otherwise>
            </xsl:choose>
          </BuySell>

          <TickerSymbol>
            <xsl:value-of select="Symbol"/>
          </TickerSymbol>

          <CusipSymbol>
            <xsl:value-of select="CUSIP"/>
          </CusipSymbol>

          <SecurityDesc>
            <xsl:choose>
              <xsl:when test="contains(SecurityDescription,',')">
                <xsl:value-of select="translate(SecurityDescription,',','')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="SecurityDescription"/>
              </xsl:otherwise>
            </xsl:choose>
          </SecurityDesc>

          <TradeQuantity>
            <xsl:value-of select="ExecutedQty"/>
          </TradeQuantity>

          <AllocatedQuantity>
            <xsl:value-of select="AllocatedQty"/>
          </AllocatedQuantity>

          <Account>
            <xsl:value-of select="AccountName"/>
          </Account>

          <Price>
            <xsl:choose>
              <xsl:when test="number(AvgPrice)">
                <xsl:value-of select="AvgPrice"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>

          </Price>

          <CmsnPerShareRate>
            <xsl:value-of select="number(CommissionCharged div ExecutedQty)"/>
          </CmsnPerShareRate>

          <Commission>
            <xsl:choose>
              <xsl:when test="number(CommissionCharged)">
                <xsl:value-of select="CommissionCharged"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </Commission>

          <xsl:variable name="varFXRate">
            <xsl:choose>
              <xsl:when test="number(FXRate_Taxlot)">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:when test="number(ForexRate)">
                <xsl:value-of select="ForexRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

       
          
          <xsl:variable name="varCommissioninBase">
            <xsl:choose>
              <xsl:when test="FXConversionMethodOperator_Taxlot='M'">
                <xsl:value-of select="(CommissionCharged*$varFXRate)"/>
              </xsl:when>
              <xsl:when test="$varFXRate!=0 and FXConversionMethodOperator_Taxlot='D'">
                <xsl:value-of select="(CommissionCharged div $varFXRate)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>           
          </xsl:variable>
          <CommissioninBase>
            <xsl:choose>
              <xsl:when test="number($varCommissioninBase)">
                <xsl:value-of select="$varCommissioninBase"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </CommissioninBase>
          <SECFees>
            <xsl:choose>
              <xsl:when test="number(StampDuty)">
                <xsl:value-of select="StampDuty"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </SECFees>

          <ORFFees>
            <xsl:choose>
              <xsl:when test="number(OrfFee)">
                <xsl:value-of select="OrfFee"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </ORFFees>

          <OCCFees>
            <xsl:choose>
              <xsl:when test="number(OccFee)">
                <xsl:value-of select="OccFee"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </OCCFees>

          <AllOtherFees>
            <xsl:choose>
              <xsl:when test="number(TransactionLevy+ClearingBrokerFee+OtherBrokerFees+ClearingFee+TaxOnCommissions+MiscFees)">
                <xsl:value-of select="TransactionLevy+ClearingBrokerFee+OtherBrokerFees+ClearingFee+TaxOnCommissions+MiscFees"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </AllOtherFees>


          <AccruedInterest>
            <xsl:value-of select="number(AccruedInterest)"/>
          </AccruedInterest>

          <NetAmountLocal>
            <xsl:choose>
              <xsl:when test="number($varNetamount)">
                <xsl:value-of select="$varNetamount"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="0"/>
              </xsl:otherwise>
            </xsl:choose>
          </NetAmountLocal>

          <FXRate>
            <xsl:choose>
              <xsl:when test="number(FXRate_Taxlot)">
                <xsl:value-of select="FXRate_Taxlot"/>
              </xsl:when>
              <xsl:when test="number(ForexRate)">
                <xsl:value-of select="ForexRate"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </FXRate>

          <NetAmountBase>
            <xsl:choose>
              <xsl:when test="number(FXRate_Taxlot)">
                <xsl:choose>
                  <xsl:when test="FXConversionMethodOperator_Taxlot='M'">
                    <xsl:value-of select="number($varNetamount*FXRate_Taxlot)"/>
                  </xsl:when>
                  <xsl:when test="FXConversionMethodOperator_Taxlot='D'">
                    <xsl:value-of select="number($varNetamount div FXRate_Taxlot)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="number($varNetamount)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="number(ForexRate)">
                <xsl:choose>
                  <xsl:when test="CurrencySymbol='GBP' or CurrencySymbol='AUD' or CurrencySymbol='EUR'">
                    <xsl:value-of select="number($varNetamount * ForexRate)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="number($varNetamount div ForexRate)"/>
                  </xsl:otherwise>
                </xsl:choose>

              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$varNetamount"/>
              </xsl:otherwise>
            </xsl:choose>
          </NetAmountBase>


          <CurrencySymbol>
            <xsl:value-of select="CurrencySymbol"/>
          </CurrencySymbol>

          <SettlementCcy>
            <xsl:choose>
              <xsl:when test="SettlCurrency != CurrencySymbol">
                <xsl:value-of select="SettlCurrency"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="CurrencySymbol"/>
              </xsl:otherwise>
            </xsl:choose>
          </SettlementCcy>

          <AssetClass>
            <xsl:value-of select="AssetClass"/>
          </AssetClass>
          
          <BorrowBroker>
            <xsl:value-of select="BorrowBroker"/>
          </BorrowBroker>

          <BorrowQty>
            <xsl:value-of select="BorrowQty"/>
          </BorrowQty>

          <BorrowID>
            <xsl:value-of select="BorrowID"/>
          </BorrowID>

          <BorrowRate>
            <xsl:value-of select="BorrowRate"/>
          </BorrowRate>

          <EntityID>
            <xsl:value-of select="EntityID"/>
          </EntityID>

        </ThirdPartyFlatFileDetail>
      </xsl:for-each>
    </ThirdPartyFlatFileDetailCollection>
  </xsl:template>
</xsl:stylesheet>
