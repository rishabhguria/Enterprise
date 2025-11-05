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
						<xsl:with-param name="Number" select="COL6"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="number($Position)">
					<PositionMaster>
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>
						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="COL7"/>
						</xsl:variable>
						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
            

						<xsl:variable name="PB_FUND_NAME" select="''"/>
						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/AccountMapping/PB[@Name=$PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="5"/>
								</xsl:when>
							</xsl:choose>
						</SideTagValue>
            
            <xsl:variable name="varCurrency" select="COL4"/>
            
            <xsl:variable name="varCostBook">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL16"/>
							</xsl:call-template>
						</xsl:variable>
            
            <xsl:variable name="varQuantity">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>
                
            <xsl:variable name="varUnitCost">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL14"/>
							</xsl:call-template>
						</xsl:variable>      
							
						<CostBasis>
							<xsl:choose>
								<xsl:when test="$varCurrency='US Dollars'">
									<xsl:value-of select="$varCostBook div $varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varUnitCost"/>
								</xsl:otherwise>
							</xsl:choose>
						</CostBasis>
            
            <FXRate>
              <xsl:choose>
								<xsl:when test="$varCurrency!='US Dollars'">
									<xsl:value-of select="($varCostBook div $varQuantity) * $varUnitCost"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="1"/>
								</xsl:otherwise>
							</xsl:choose>
            </FXRate>
            
						<PBSymbol>
							<xsl:value-of select ="$PB_SYMBOL_NAME"/>
						</PBSymbol>
            
						<xsl:variable name ="Date" select="COL13"/>
						<OriginalPurchaseDate>
							<xsl:value-of select="$Date"/>
						</OriginalPurchaseDate>
					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
</xsl:stylesheet>