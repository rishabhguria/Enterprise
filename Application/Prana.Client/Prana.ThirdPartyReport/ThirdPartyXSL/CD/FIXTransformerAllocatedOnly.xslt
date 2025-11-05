<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<!--TaxLotStateID=0 means Allocated i.e. Trade comes first time -->
	<!--TaxLotStateID=1 means Sent i.e. Trade sent to PB -->
	<!--TaxLotStateID=2 means Amended i.e. Trade amended after sending to PB -->
	<!--TaxLotStateID=3 means Deleted i.e. Trade unallocated or deleted after sending to PB -->
	<!--TaxLotStateID=4 means Ignore i.e.we can set a trade to ignore, it will come in the different tab-->
	<!--TranCode = ';'  means previous day buy transaction closes with today's sell transaction-->
	<xsl:template match="/">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>
			<xsl:for-each select="//ThirdPartyFlatFileDetail">
				<xsl:choose>			
					
					<xsl:when test ="TaxLotFIXAckStateID!=0 and TaxLotFIXStateID=0">
						<ThirdPartyFlatFileDetail>
							<!-- system inetrnal use-->
              <EntityID>
                <xsl:value-of select="EntityID"/>
              </EntityID>
							<TaxLotFIXState>
                <xsl:value-of select="TaxLotFIXState"/>								
							</TaxLotFIXState>

              <TaxLotState>
                <xsl:value-of select="TaxLotState"/>
              </TaxLotState>
              
              <TaxLotFIXStateID>
                <xsl:value-of select="TaxLotFIXStateID"/>							
							</TaxLotFIXStateID>

              <TaxLotFIXAckState>
                <xsl:value-of select="TaxLotFIXAckState"/>
              </TaxLotFIXAckState>

              <TaxLotFIXAckStateID>
                <xsl:value-of select="TaxLotFIXAckStateID"/>
              </TaxLotFIXAckStateID>

              <!-- This column is used to grouping the rows on  UI-->
              <TaxlotFIXStateCombined>
                <xsl:value-of select="concat(TaxLotFIXState,'-',TaxLotFIXAckState)"/>
              </TaxlotFIXStateCombined>
              
              <TaxLotPK>
                <xsl:value-of select="EntityID"/>
              </TaxLotPK>
              <TaxLotId>
                <xsl:value-of select="EntityID"/>
              </TaxLotId>
              
              <OriginatorType>
                <xsl:value-of select="'4'"/>
              </OriginatorType>
              

              <!--<TaxLotState>
								<xsl:value-of select ="'Allocated'"/>
							</TaxLotState>-->
							<!-- system inetrnal use-->
							<RowHeader>
								<xsl:value-of select ="'false'"/>
							</RowHeader>

							<!--<TaxLot>
						<xsl:value-of select="Level1AllocationID"/>
					</TaxLot>-->

							<!-- COL1-->
              <AllocID>
								<xsl:value-of select="EntityID"/>
							</AllocID>
							<!-- COL2-->
              <AllocTransType>
                <xsl:choose>
                  <xsl:when test ="TaxLotFIXStateID=0">
                    <xsl:value-of select ="'0'"/>
                  </xsl:when>                
                  <xsl:when test ="TaxLotFIXStateID=2">
                    <xsl:value-of select ="'1'"/>
                  </xsl:when>
                  <xsl:when test ="TaxLotFIXStateID=3">
                    <xsl:value-of select ="'2'"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select ="'0'"/>
                  </xsl:otherwise>
                </xsl:choose>
							</AllocTransType>
							<!-- COL3-->
              <RefAllocID>
								<xsl:value-of select="TradeRefID"/>
							</RefAllocID>
							<!-- COL4-->
              <AllocLinkID>
								<xsl:value-of select="TradeRefID"/>
							</AllocLinkID>
							<!-- COL5-->
              <AllocLinkType>
								<xsl:value-of select="'0'"/>
							</AllocLinkType>
							<!-- COL6-->
              <ClOrdID>
								<xsl:value-of select="TradeRefID"/>
							</ClOrdID>
							<!-- COL7-->
              <OrderID>
								<xsl:value-of select="TradeRefID"/>
							</OrderID>
							<!-- COL8-->
              <Side>
								<xsl:value-of select="SideTag"/>
							</Side>
							<!-- COL9-->
              <Symbol>
                <xsl:value-of select="Symbol"/>
							</Symbol>
							<!-- COL10-->
              <Shares>
								<xsl:value-of select="TotalQty"/>
							</Shares>
							<!-- COL11-->
              <AvgPrice>
								<xsl:value-of select="AveragePrice"/>
							</AvgPrice>
							<!-- COL12-->
              <TradeDate>
								<xsl:value-of select="TradeDate"/>
							</TradeDate>
							<!-- COL13-->

              <AllocAccount>
                <xsl:value-of select="AccountAccountNo"/>
              </AllocAccount>
              <AllocShares>
								<xsl:value-of select="AllocatedQty"/>
							</AllocShares>
              <!-- COL14-->
              <OrderSideTagValue>
                <xsl:value-of select="SideTag"/>
              </OrderSideTagValue>
						</ThirdPartyFlatFileDetail>
					</xsl:when>
					<!--<xsl:otherwise>
					
					</xsl:otherwise>-->
				</xsl:choose>
			</xsl:for-each>			
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
	
	<!-- variable declaration for lower to upper case -->
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<!-- variable declaration for lower to upper case ENDs -->
	
</xsl:stylesheet>
