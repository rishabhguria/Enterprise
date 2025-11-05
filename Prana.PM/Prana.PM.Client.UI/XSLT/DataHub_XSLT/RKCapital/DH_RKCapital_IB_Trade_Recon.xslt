<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here" version="1.0">
	<xsl:output method="xml" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="/">
		
		<DocumentElement>
			
			<xsl:for-each select="//Comparision">
				
				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL16)"/>
					</xsl:call-template>
				</xsl:variable>
				
				<xsl:choose>
					
					<xsl:when test="number($varQuantity)">
						
						<PositionMaster>
							
							<xsl:variable name="PB_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>
							
							<xsl:variable name="varSymbol">
								<xsl:value-of select="normalize-space(COL5)"/>
							</xsl:variable>
							
							<xsl:variable name="PB_SYMBOL_NAME">
								<xsl:value-of select="''"/>
							</xsl:variable>
							
							<xsl:variable name="PRANA_SYMBOL_NAME">
								<xsl:value-of select="document('../../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
							</xsl:variable>
							
							<Symbol>
								<xsl:choose>
									<xsl:when test="$PRANA_SYMBOL_NAME!=''">
										<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
									</xsl:when>									
									<xsl:when test="$varSymbol !='' and $varSymbol !='*'">
										<xsl:value-of select="$varSymbol"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_SYMBOL_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</Symbol>
							
							<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL2)"/>
							
							<xsl:variable name="PRANA_FUND_NAME">
								<xsl:value-of select="document('../../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
							</xsl:variable>
							
							<FundName>
								<xsl:choose>
									<xsl:when test="$PRANA_FUND_NAME!=''">
										<xsl:value-of select="$PRANA_FUND_NAME"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PB_FUND_NAME"/>
									</xsl:otherwise>
								</xsl:choose>
							</FundName>
							
							<xsl:variable name="varSide" select="normalize-space(COL15)"/>
							
							<Side>								
								<xsl:choose>
									<xsl:when test="$varSide = 'BUY'">
										<xsl:value-of select="'Buy'"/>
									</xsl:when>
									<xsl:when test="$varSide = 'SELL'">
										<xsl:value-of select="'Sell'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="''"/>
									</xsl:otherwise>
								</xsl:choose>									
							</Side>
							
							<Quantity>
								<xsl:choose>
									<xsl:when test="$varQuantity &gt;0">
										<xsl:value-of select="$varQuantity"/>
									</xsl:when>
									<xsl:when test="$varQuantity &lt;0">
										<xsl:value-of select="$varQuantity * -1"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Quantity>
							
							<xsl:variable name="varCurrencySymbol">
								<xsl:value-of select="normalize-space(COL10)"/>
							</xsl:variable>
							
							<CurrencySymbol>
								<xsl:value-of select="$varCurrencySymbol"/>
							</CurrencySymbol>
							
							<xsl:variable name="AvgPrice">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL17)"/>
								</xsl:call-template>
							</xsl:variable>
							
							<AvgPX>
								<xsl:choose>
									<xsl:when test="$AvgPrice  &gt;0">
										<xsl:value-of select="$AvgPrice"/>
									</xsl:when>
									<xsl:when test="$AvgPrice &lt;0">
										<xsl:value-of select="$AvgPrice * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</AvgPX>
							
							<xsl:variable name="Commission">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL20)"/>
								</xsl:call-template>
							</xsl:variable>
							
							<Commission>
								<xsl:choose>
									<xsl:when test="$Commission  &gt;0">
										<xsl:value-of select="$Commission"/>
									</xsl:when>
									<xsl:when test="$Commission &lt;0">
										<xsl:value-of select="$Commission * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</Commission>
							
							<xsl:variable name="SecFee">
								<xsl:call-template name="Translate">
									<xsl:with-param name="Number" select="normalize-space(COL19)"/>
								</xsl:call-template>
							</xsl:variable>
							
							<SecFee>
								<xsl:choose>
									<xsl:when test="$SecFee &gt;0">
										<xsl:value-of select="$SecFee"/>
									</xsl:when>
									<xsl:when test="$SecFee &lt;0">
										<xsl:value-of select="$SecFee * (-1)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="0"/>
									</xsl:otherwise>
								</xsl:choose>
							</SecFee>
							
							<TradeDate>
								<xsl:value-of select="normalize-space(COL12)"/>
							</TradeDate>
							
							<SettlementDate>
								<xsl:value-of select="normalize-space(COL14)"/>
							</SettlementDate>
							
							<SMRequest>
								<xsl:value-of select="'True'"/>
							</SMRequest>
							
						</PositionMaster>
					</xsl:when>
					<xsl:otherwise>
						<PositionMaster>
							<Symbol>
								<xsl:value-of select="''"/>
							</Symbol>
							<FundName>
								<xsl:value-of select="''"/>
							</FundName>
							<Side>
								<xsl:value-of select="''"/>
							</Side>
							<Quantity>
								<xsl:value-of select="0"/>
							</Quantity>
							<CurrencySymbol>
								<xsl:value-of select="''"/>
							</CurrencySymbol>
							<AvgPX>
								<xsl:value-of select="0"/>
							</AvgPX>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
							<SecFee>
								<xsl:value-of select="0"/>
							</SecFee>
							<TradeDate>
								<xsl:value-of select="''"/>
							</TradeDate>
							<SettlementDate>
								<xsl:value-of select="''"/>
							</SettlementDate>
							<SMRequest>
								<xsl:value-of select="'True'"/>
							</SMRequest>
						</PositionMaster>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>