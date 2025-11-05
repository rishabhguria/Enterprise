<?xml version="1.0" encoding="UTF-8"?>
											
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <xsl:template match="/ThirdPartyFlatFileDetailCollection">
    <ThirdPartyFlatFileDetailCollection>
      <xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

		<ThirdPartyFlatFileDetail>
			<!--for system internal use-->
			<RowHeader>
				<xsl:value-of select ="'true'"/>
			</RowHeader>

      <!--for system use only-->
      <IsCaptionChangeRequired>
        <xsl:value-of select ="'true'"/>
      </IsCaptionChangeRequired>

      <!--for system internal use-->
      <TaxLotState>
        <xsl:value-of select ="'TaxLotState'"/>
      </TaxLotState>

			<AccountNumber>
				<xsl:value-of select ="'AccountNumber'"/>
			</AccountNumber>

			<Operation>
				<xsl:value-of select ="'Operation'"/>
			</Operation>

			<SettlementCurrency>
				<xsl:value-of select ="'SettlementCurrency'"/>
			</SettlementCurrency>

			<Quantity>
				<xsl:value-of select ="'Quantity'"/>
			</Quantity>

			<GiveInPrice>
				<xsl:value-of select ="'GiveInPrice'"/>
			</GiveInPrice>

			<SecurityIdentifierType>
				<xsl:value-of select ="'SecurityIdentifierType'"/>
			</SecurityIdentifierType>

			<Security>
				<xsl:value-of select ="'Security'"/>
			</Security>

			<TradeDate>
				<xsl:value-of select="'TradeDate'"/>
			</TradeDate>

			<SettlementDate>
				  <xsl:value-of select ="'SettlementDate'"/>
			</SettlementDate>

			<Commission>
				  <xsl:value-of select ="'Commission'"/>
			 </Commission>

			<DealRef>
				  <xsl:value-of select ="'DealRef'"/>
			</DealRef>

			<GiveUpBroker>
				  <xsl:value-of select ="'GiveUpBroker'"/>
			</GiveUpBroker>

			<ProductType>
				  <xsl:value-of select="'ProductType'"/>
		    </ProductType>


			<TransactionType>
				  <xsl:value-of select ="'TransactionType'"/>
			</TransactionType>

			<Comment>
				  <xsl:value-of select ="'Comment'"/>
			</Comment>

      <!-- system use only-->
      <EntityID>
        <xsl:value-of select="'EntityID'"/>
      </EntityID>
        
		</ThirdPartyFlatFileDetail>
		
      <xsl:for-each select="ThirdPartyFlatFileDetail">
		  <xsl:if test ="AccountNo='013101969' or AccountNo='013425764'or AccountNo='013216619' or AccountNo='049031727'">		  
		  <ThirdPartyFlatFileDetail>
			  <!--for system internal use-->
			  <RowHeader>
				  <xsl:value-of select ="'true'"/>
			  </RowHeader>

			  <!--for system use only-->
			  <IsCaptionChangeRequired>
				  <xsl:value-of select ="'true'"/>
			  </IsCaptionChangeRequired>

			  <!--for system internal use-->
			  <TaxLotState>
				  <xsl:value-of select ="TaxLotState"/>
			  </TaxLotState>

			  <AccountNumber>
				  <xsl:value-of select ="AccountNo"/>
			  </AccountNumber>

			  <xsl:variable name="NormalizedFxRate">
				  <xsl:choose>
					  <xsl:when test="FXRate_Taxlot='' or FXRate_Taxlot='0' or AccountNo = '002276293' or AccountNo = '002200079' or AccountNo = '002200640' or AccountNo = '002201481'">
						  <xsl:value-of select="1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="FXRate_Taxlot"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>

			  <xsl:variable name="SideMultiplier">
				  <xsl:choose>
					  <xsl:when test="Side='Buy' or Side='Buy to Open' or Side='Buy to Close' or Side='Buy to Cover'">
						  <xsl:value-of select="1"/>
					  </xsl:when>
					  <xsl:when test="Side='Sell' or Side='Sell to Close' or Side='Sell short' or Side='Sell to Open'">
						  <xsl:value-of select="-1"/>
					  </xsl:when>
					  <xsl:otherwise>
						  <xsl:value-of select="1"/>
					  </xsl:otherwise>
				  </xsl:choose>
			  </xsl:variable>
			  
			  <xsl:variable name="varSecFees">
				  <xsl:value-of select="StampDuty + TransactionLevy + OtherBrokerFee + ClearingFee + MiscFees + TaxOnCommissions"/>
			  </xsl:variable>
			  
			  <xsl:variable name="NormalizedAveragePrice">
				  <xsl:value-of select="AveragePrice + $SideMultiplier * ((CommissionCharged+$varSecFees)div ExecutedQty)"/>
			  </xsl:variable>			  
			  <!--<xsl:choose>
				  <xsl:when test="Side='Buy' or 'Buy to Open' or Side='Buy to Close'">
					  <Operation>
						  <xsl:value-of select ="substring(Side,1,1)"/>
					  </Operation>
				  </xsl:when>
				  <xsl:when test="Side='Sell' or 'Sell to Close' or Side='Sell short' or 'Sell to Open'">
					  <Operation>
						  <xsl:value-of select ="'S'"/>
					  </Operation>
				  </xsl:when>
				  <xsl:otherwise>-->					  
					  <Operation>
						  <xsl:value-of select ="substring(Side,1,1)"/>
					  </Operation>					  
				  <!--</xsl:otherwise>
			  </xsl:choose>-->
			  
			  <SettlementCurrency>
				  <xsl:value-of select ="'USD'"/>
			  </SettlementCurrency>

			
			  <Quantity>
				  <xsl:value-of select ="AllocatedQty"/>
			  </Quantity>

			  <GiveInPrice>
				  <!--<xsl:value-of select ="$NormalizedAveragePrice*$NormalizedFxRate"/>-->
				  <xsl:value-of select="format-number($NormalizedAveragePrice*$NormalizedFxRate,'0.0000')"/>
			  </GiveInPrice>

			  <SecurityIdentifierType>
				  <xsl:value-of select ="'Sedol'"/>
			  </SecurityIdentifierType>
			 
			  <Security>
				  <xsl:value-of select ="SEDOL"/>
			  </Security>

			  <TradeDate>
				  <xsl:value-of select="TradeDate"/>
			  </TradeDate>

			  <SettlementDate>
				  <xsl:value-of select ="SettlementDate"/>
			  </SettlementDate>

			  <Commission>
				  <xsl:value-of select ="CommissionCharged*$NormalizedFxRate"/>
			  </Commission>

			  <!--Not clear-->
			  <DealRef>
				  <xsl:value-of select ="''"/>
			  </DealRef>

			  <!--<GiveUpBroker>
				  <xsl:value-of select ="CounterParty"/>
			  </GiveUpBroker>-->

			  <!--Not clear-->
			  <xsl:choose>
				  <xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
					  <GiveUpBroker>
						  <xsl:value-of select="'GS'"/>
					  </GiveUpBroker>
				  </xsl:when>
				  <xsl:when test ="CounterParty='UBS Program' or CounterParty='UBS Electronic'">
					  <GiveUpBroker>
						  <xsl:value-of select="'EFUBS'"/>
					  </GiveUpBroker>
				  </xsl:when>
				  <xsl:when test ="CounterParty='DBPrg' or CounterParty='DBElec'">
					  <GiveUpBroker>
						  <xsl:value-of select="'EFDUET'"/>
					  </GiveUpBroker>
				  </xsl:when>

				  
				  <xsl:when test ="CounterParty='INSTElec'">
								<GiveUpBroker>
				  <xsl:value-of select="'EFINST'"/>
					  </GiveUpBroker>
				  </xsl:when>
				  <xsl:otherwise >
					  <GiveUpBroker>
						  <xsl:value-of select="CounterParty"/>
					  </GiveUpBroker>
				  </xsl:otherwise>
			  </xsl:choose >

			  <ProductType>
				  <xsl:value-of select="'CFD'"/>
			  </ProductType>
			  
			  <xsl:choose>
				  <xsl:when test="TaxLotState='Allocated'">
					  <TransactionType>
						  <xsl:value-of select ="'New'"/>
					  </TransactionType>
				  </xsl:when>
				  <xsl:when test="TaxLotState='Amemded'">
					  <TransactionType>
						  <xsl:value-of select ="'Amend'"/>
					  </TransactionType>
				  </xsl:when>				  
				  <xsl:when test="TaxLotState='Deleted'">
					  <TransactionType>
						  <xsl:value-of select ="'Cancel'"/>
					  </TransactionType>
				  </xsl:when>
				  <xsl:otherwise>
					  <TransactionType>
						  <xsl:value-of select ="'New'"/>
					  </TransactionType>
				  </xsl:otherwise>
			  </xsl:choose>			 

			  <Comment>
				  <xsl:value-of select ="''"/>
			  </Comment>

        <!-- System internal use only-->
        <EntityID>
          <xsl:value-of select="EntityID"/>
        </EntityID>

      </ThirdPartyFlatFileDetail>
		  </xsl:if>  
	  </xsl:for-each>
  </ThirdPartyFlatFileDetailCollection>
</xsl:template>
</xsl:stylesheet>
