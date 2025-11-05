<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" version="1.0" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/NewDataSet">
		<ThirdPartyFlatFileDetailCollection>
			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>
					<!-- for system internal use -->
					<RowHeader>
						<xsl:value-of select="'true'"/>
					</RowHeader>
					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>

					<PortfolioName>
						<xsl:value-of select="'MASTER'"/>
					</PortfolioName>

					<PortfolioDescription>
						<xsl:value-of select="MasterFund"/>
					</PortfolioDescription>

					<Date>
						<xsl:value-of select ="TradeDate"/>
					</Date>

					<Symbol>
                       <xsl:choose>  
							 <xsl:when test="AssetClass =  'Cash'">
                               <xsl:value-of select="Symbol"/> 
                            </xsl:when>	
							<xsl:when test="AssetClass =  'EquityOption'">
                               <xsl:value-of select="OSISymbol"/>
                            </xsl:when>								
							
							
                            <xsl:when test="BloombergSymbol!='' or BloombergSymbol!='*'">
                                <xsl:value-of select="normalize-space(substring-before(BloombergSymbol,'Equity'))"/>
                            </xsl:when>
							
                            
                            <xsl:otherwise>
                                <xsl:value-of select="Symbol"/>
                            </xsl:otherwise>
                        </xsl:choose>
                    </Symbol>

					<SecurityName>
						<xsl:value-of select="SecurityName"/>
					</SecurityName>
					
					<Quantity>
						<xsl:value-of select="OpenQty"/>
					</Quantity>
					
					<Price>
					  <xsl:value-of select="MarkPrice"/>
					</Price>
					
					<PriceISO>						
				    	<xsl:value-of select="TradeCurrency"/>
					</PriceISO>
					
					<AssetClass>
						<xsl:choose>
							<xsl:when test="AssetClass ='Equity'">
								<xsl:value-of select="'EQUITY'"/>
							</xsl:when>
							<xsl:when test="AssetClass =  'EquityOption'">
                               <xsl:value-of select="'OPTIONS'"/> 
                            </xsl:when>	
							<xsl:when test="AssetClass ='Cash'">
								<xsl:value-of select="'FX'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
					</AssetClass>
					
					<AssetType>
						<xsl:choose>
							<xsl:when test="AssetClass ='Equity'">
								<xsl:value-of select="'COMMON'"/>
							</xsl:when>
							<xsl:when test="AssetClass =  'EquityOption'">
								<xsl:choose>
									<xsl:when test="PutOrCall='PUT'">
										<xsl:value-of select="'EQUITY OPTION: PUT'"/>
									</xsl:when>
									<xsl:when test="PutOrCall='CALL'">
										<xsl:value-of select="'EQUITY OPTION: CALL'"/>
									</xsl:when>
								</xsl:choose>
                              
                            </xsl:when>
							<xsl:when test="AssetClass ='Cash'">
								<xsl:value-of select="'CASH'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</AssetType>
					
					<ISIN>
						<xsl:value-of select="ISINSymbol"/>
					</ISIN>
					
					<CUSIP>
						<xsl:value-of select="CUSIPSymbol"/>
					</CUSIP>
					
				
					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>
				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>
</xsl:stylesheet>