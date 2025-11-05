<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
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
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="Quantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL12"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Quantity)">
					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="COL7"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>


						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL2)" />
						</xsl:variable>
					
						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
							
								<xsl:when test="$varSymbol!='' or $varSymbol!='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
					     	<xsl:variable name="PB_FUND_NAME" select="'Cowen P3K009360'"/>
						
					     <xsl:variable name="PRANA_FUND_NAME">
					       <xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					     </xsl:variable>
						
					     <AccountName>
					       <xsl:choose>
					         <xsl:when test="$PRANA_FUND_NAME!=''">
					           <xsl:value-of select="$PRANA_FUND_NAME"/>
					         </xsl:when>
					         <xsl:otherwise>
					           <xsl:value-of select="$PB_FUND_NAME"/>
					         </xsl:otherwise>
					       </xsl:choose>
					     </AccountName>

						
		   
				       <!-- <TradeDate> -->
							<!-- <xsl:value-of select="COL56"/> -->
						<!-- </TradeDate> -->


						<Quantity>
				          <xsl:choose>
				            <xsl:when test="number($Quantity)">
				              <xsl:value-of select="$Quantity"/>
				            </xsl:when>
				            <xsl:otherwise>
				              <xsl:value-of select="0"/>
				            </xsl:otherwise>
				          </xsl:choose>
				        </Quantity>

				      
				        <Side>
				          <xsl:choose>
				            <xsl:when test="$Quantity &gt; 0">
				              <xsl:value-of select="'Buy'"/>
				            </xsl:when>
				            <xsl:when test="$Quantity &lt; 0">
				              <xsl:value-of select="'Sell short'"/>
				            </xsl:when>
				            <xsl:otherwise>
				              <xsl:value-of select="''"/>
				            </xsl:otherwise>
				          </xsl:choose>       
				        </Side>
						
						<xsl:variable name="MarkPrice">
				          <xsl:call-template name="Translate">
				            <xsl:with-param name="Number" select="COL16"/>
				          </xsl:call-template>
				        </xsl:variable>
				        <MarkPrice>
				          <xsl:choose>
				            <xsl:when test="number($MarkPrice)">
				              <xsl:value-of select="$MarkPrice"/>
				            </xsl:when>
				            <xsl:otherwise>
				              <xsl:value-of select="0"/>
				            </xsl:otherwise>
				          </xsl:choose>
				        </MarkPrice>
									
						<xsl:variable name="MarketValue">
				          <xsl:call-template name="Translate">
				            <xsl:with-param name="Number" select="COL9"/>
				          </xsl:call-template>
				        </xsl:variable>
				        <MarketValue>
				          <xsl:choose>
				            <xsl:when test="number($MarketValue)">
				              <xsl:value-of select="$MarketValue"/>
				            </xsl:when>
				            <xsl:otherwise>
				              <xsl:value-of select="0"/>
				            </xsl:otherwise>
				          </xsl:choose>
				        </MarketValue>

				        <xsl:variable name="MarketValueBase">
				          <xsl:call-template name="Translate">
				            <xsl:with-param name="Number" select="COL14"/>
				          </xsl:call-template>
				        </xsl:variable>
				        <MarketValueBase>
				          <xsl:choose>
				            <xsl:when test="number($MarketValueBase)">
				              <xsl:value-of select="$MarketValueBase"/>
				            </xsl:when>
				            <xsl:otherwise>
				              <xsl:value-of select="0"/>
				            </xsl:otherwise>
				          </xsl:choose>
				        </MarketValueBase>
						<PBSymbol>
				          <xsl:value-of select="$PB_SYMBOL_NAME"/>
				        </PBSymbol>
						
						<CurrencySymbol>
						 <xsl:value-of select="COL8"/>
						</CurrencySymbol>

          </PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet>


