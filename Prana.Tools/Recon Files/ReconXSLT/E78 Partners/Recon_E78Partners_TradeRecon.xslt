<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL10"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity) ">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						
						<xsl:variable name="varCUSIP">
							<xsl:value-of select="normalize-space(COL6)"/>
						</xsl:variable>
					
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varCUSIP!=''">
									<xsl:value-of select="''"/>
								</xsl:when>							
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<CUSIP>
							<xsl:choose>
								<xsl:when test="$varCUSIP!=''">
									<xsl:value-of select="$varCUSIP"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</CUSIP>
						<xsl:variable name="PB_FUND_NAME" select="''"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>
						<AccountName>
							<xsl:choose>
								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</AccountName>

						<Quantity>
							<xsl:choose>
								<xsl:when test="$Quantity &gt; 0">
									<xsl:value-of select="$Quantity"/>
								</xsl:when>
								<xsl:when test="$Quantity &lt; 0">
									<xsl:value-of select="$Quantity * -1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="AvgPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL11"/>
							</xsl:call-template>
						</xsl:variable>

						<AvgPX>
							<xsl:choose>
								<xsl:when test="$AvgPrice &gt; 0">
									<xsl:value-of select="$AvgPrice"/>
								</xsl:when>
								<xsl:when test="$AvgPrice &lt; 0">
									<xsl:value-of select="$AvgPrice * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</AvgPX>
					 <xsl:variable name="varCommission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="''"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							<xsl:choose>
								<xsl:when test="$varCommission &gt; 0">
									<xsl:value-of select="$varCommission"/>
								</xsl:when>
								<xsl:when test="$varCommission &lt; 0">
									<xsl:value-of select="$varCommission * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Commission>

						<xsl:variable name="Side">
							<xsl:value-of select="COL13"/>
						</xsl:variable>
						<Side>
						<xsl:choose>
								<xsl:when test="$varAsset='EquityOption'">
									<xsl:choose>
										<xsl:when test="$Side = 'Buy'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="$Side ='Sell'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:when test="$Side = 'Cover Short'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when test="$Side = 'Sell Short'">
											<xsl:value-of select="'Sell to Open'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$Side = 'Buy'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="$Side = 'Sell'">
											<xsl:value-of select="'Sell'"/>
										</xsl:when>
										<xsl:when test="$Side = 'Cover Short'">
											<xsl:value-of select="'Buy to Close'"/>
										</xsl:when>
										<xsl:when test="$Side = 'Sell Short'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>							
						</Side>					

						<TradeDate>
							<xsl:value-of select="COL2"/>
						</TradeDate>
						
						<SettlementDate>
							<xsl:value-of select="COL3"/>
						</SettlementDate>

						

                         <CurrencySymbol>
                             <xsl:value-of select="COL9"/>
                          </CurrencySymbol>
				
						<PBSymbol>
							<xsl:value-of select="$PB_SYMBOL_NAME"/>
						</PBSymbol>					

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->
</xsl:stylesheet>



