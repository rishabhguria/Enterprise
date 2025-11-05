<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">
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

	<xsl:template name="spaces">
		<xsl:param name="count"/>
		<xsl:if test="number($count)">
			<xsl:call-template name="spaces">
				<xsl:with-param name="count" select="$count - 1"/>
			</xsl:call-template>
		</xsl:if>
		<xsl:value-of select="' '"/>
	</xsl:template>

	<xsl:template match="/">
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL19"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position) and (COL7='Buy' or COL7='CoverShort' or COL7='SellShort' or COL7='Sell')">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'UBS'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL14"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol">
									<xsl:value-of select="COL10"/>
						</xsl:variable>
            
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
                
								<xsl:when test="$Symbol!=''">
									<xsl:value-of select="$Symbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>

						</Symbol>

            <xsl:variable name="PB_FUND_NAME" select="COL3"/>

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

						<NetPosition>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>

						<xsl:variable name="Side" select="COL7"/>
						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Side='Buy'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:when test="$Side='CoverShort'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
								<xsl:when test="$Side='SellShort'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="$Side='Sell'">
									<xsl:value-of select="'2'"/>
								</xsl:when>			
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</SideTagValue>


						<xsl:variable name="varEndingLocal">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL21"/>
							</xsl:call-template>
						</xsl:variable>

						<xsl:variable name="Costbasis">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="$varEndingLocal div $Position"/>
							</xsl:call-template>
						</xsl:variable>
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varEndingLocal &gt; 0">
									<xsl:value-of select="$varEndingLocal"/>
								</xsl:when>

								<xsl:when test="$varEndingLocal &lt; 0">
									<xsl:value-of select="$varEndingLocal * (-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>


						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>

						<xsl:variable name="varPositionStartDate">
							<xsl:value-of select="COL5"/>
						</xsl:variable>
						<PositionStartDate>
							<xsl:value-of select="$varPositionStartDate"/>
						</PositionStartDate>
						
             <PositionSettlementDate>
							<xsl:value-of select="COL6"/>
						</PositionSettlementDate>
              
            <Commission>
							<xsl:value-of select="COL27"/>
						</Commission>

						<SecFee>
							<xsl:value-of select="COL28"/>
						</SecFee>
						
						<Fees>
							<xsl:value-of select="COL30"/>
						</Fees>
						
           <xsl:variable name="PB_BROKER_NAME">
              <xsl:value-of select="normalize-space(COL33)"/>
              </xsl:variable>

              <xsl:variable name="PRANA_BROKER_ID">
                <xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
              </xsl:variable>

                    <CounterPartyID>
                          <xsl:choose>
                            <xsl:when test="number($PRANA_BROKER_ID)">
                              <xsl:value-of select="$PRANA_BROKER_ID"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="0"/>
                            </xsl:otherwise>
                          </xsl:choose>
                    </CounterPartyID>
					</PositionMaster>
				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>