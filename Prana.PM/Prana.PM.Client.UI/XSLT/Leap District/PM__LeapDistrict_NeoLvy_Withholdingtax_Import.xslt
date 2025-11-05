<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:my1="put-your-namespace-uri-here"
  xmlns:my2="put-your-namespace-uri-here">

  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="Translate">
		<xsl:param name="Number" />
		<xsl:variable name="SingleQuote">'</xsl:variable>
		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))" />
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

<xsl:template name="MonthCode1">
                <xsl:param name="Month"/>
                <xsl:choose>
                        <xsl:when test="$Month='Jan'">
                                <xsl:value-of select="'01'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Feb'">
                                <xsl:value-of select="'02'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Mar'">
                                <xsl:value-of select="'03'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Apr'">
                                <xsl:value-of select="'04'"/>
                        </xsl:when>
                        <xsl:when test="$Month='May'">
                                <xsl:value-of select="'05'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Jun'">
                                <xsl:value-of select="'06'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Jul'">
                                <xsl:value-of select="'07'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Aug'">
                                <xsl:value-of select="'08'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Sep'">
                                <xsl:value-of select="'09'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Oct'">
                                <xsl:value-of select="'10'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Nov'">
                                <xsl:value-of select="'11'"/>
                        </xsl:when>
                        <xsl:when test="$Month='Dec'">
                                <xsl:value-of select="'12'"/>
                        </xsl:when>
                        <xsl:otherwise>
                                <xsl:value-of select="''"/>
                        </xsl:otherwise>
                </xsl:choose>
        </xsl:template>
		<xsl:template match="/">
		
		<DocumentElement>
			<xsl:for-each select="//PositionMaster">

				<xsl:variable name="varAmount">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="normalize-space(COL19)"/>
					</xsl:call-template>
				</xsl:variable>		

				<xsl:if test="number($varAmount)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						 <xsl:variable name="PB_FUND_NAME">
							 <xsl:value-of select="COL2"/>
						 </xsl:variable>
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
						<xsl:variable name="PB_SYMBOL_NAME" select="COL7"/>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name = $PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>
						<xsl:variable name="varSymbol" select="normalize-space(COL7)"/>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varSymbol !='' or $varSymbol !='*'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>
						
						<Amount>
							<xsl:choose>
								<xsl:when test="number($varAmount)">
									<xsl:value-of select="$varAmount"/>
								</xsl:when>
								
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Amount>
						
							  <xsl:variable name="varMM">
					            <xsl:call-template name="MonthCode1">
					             <xsl:with-param name="Month" select="substring-before(substring-after(COL76,'-'),'-')"/>
					           </xsl:call-template>
					         </xsl:variable>
					                     
					          <xsl:variable name="varDD">
					            <xsl:value-of select="substring-before(normalize-space(COL76),'-')" />
					         </xsl:variable>
					          <xsl:variable name="varYYYY">
					            <xsl:value-of select="substring-after(substring-after(normalize-space(COL76),'-'),'-')" />
					         </xsl:variable>

						<ExDate>
					 <xsl:value-of select ="COL9"/>
						</ExDate>
						
						<RecordDate>
							<xsl:value-of select="''" />
						</RecordDate>
						  <xsl:variable name="varMM1">
					            <xsl:call-template name="MonthCode1">
					             <xsl:with-param name="Month" select="substring-before(substring-after(COL78,'-'),'-')"/>
					           </xsl:call-template>
					         </xsl:variable>
					                     
					          <xsl:variable name="varDD1">
					            <xsl:value-of select="substring-before(normalize-space(COL78),'-')" />
					         </xsl:variable>
					          <xsl:variable name="varYYYY1">
					            <xsl:value-of select="substring-after(substring-after(normalize-space(COL78),'-'),'-')" />
					         </xsl:variable>
						<PayoutDate>
							 <xsl:value-of select ="COL11"/>
						</PayoutDate>
						
						<CurrencyID>
								<xsl:value-of select="1"/>
						</CurrencyID>
					

						<ActivityType>
							<xsl:choose>							
								
								<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="'WithholdingTax'"/>
								</xsl:when>									
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</ActivityType>
						
						<Description>
							<xsl:choose>							
								
						<xsl:when test="$varAmount &lt;0">
									<xsl:value-of select="'Withholding Tax'"/>
								</xsl:when>									
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</Description>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>