<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here">

	<xsl:output method="xml" indent="yes"/>

	
	<msxsl:script language="C#" implements-prefix="my">
		public string Now(int year, int month)
		{
		DateTime thirdFriday= new DateTime(year, month, 15);
		while (thirdFriday.DayOfWeek != DayOfWeek.Friday)
		{
		thirdFriday = thirdFriday.AddDays(1);
		}
		return thirdFriday.ToString();
		}
	</msxsl:script>


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

			<xsl:for-each select ="//PositionMaster">
        
                <xsl:variable name="varQuantity">
                  <xsl:call-template name="Translate">
                    <xsl:with-param name="Number" select="COL8"/>
                  </xsl:call-template>
                </xsl:variable>

				<xsl:if test="number($varQuantity)">
					
					<PositionMaster>
						
						<xsl:variable name="PB_NAME">
							<xsl:value-of select="''"/>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Asset">														
							 <xsl:value-of select="'Future'"/>							
						</xsl:variable>
						
						<TickerSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</TickerSymbol>

						<UnderLyingSymbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$Asset='Future'">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</UnderLyingSymbol>
											     
						
					   <xsl:variable name="varCurrencyI">
							<xsl:choose>
								<xsl:when test="COL19='HKD'">
									<xsl:value-of select="2"/>
								</xsl:when>
								<xsl:when test="COL19='USD'">
									<xsl:value-of select="1"/>
								</xsl:when>
								<xsl:when test="COL19='AUD'">
									<xsl:value-of select="14"/>
								</xsl:when>
								<xsl:when test="COL19='CAD'">
									<xsl:value-of select="7"/>
								</xsl:when>
								<xsl:when test="COL19='JPY'">
									<xsl:value-of select="3"/>
								</xsl:when>
								<xsl:when test="COL19='GBP'">
									<xsl:value-of select="4"/>
								</xsl:when>
								<xsl:when test="COL19='AED'">
									<xsl:value-of select="5"/>
								</xsl:when>
								<xsl:when test="COL19='BRL'">
									<xsl:value-of select="6"/>
								</xsl:when>
								<xsl:when test="COL19='EUR'">
									<xsl:value-of select="8"/>
								</xsl:when>
								<xsl:when test="COL19='NOK'">
									<xsl:value-of select="9"/>
								</xsl:when>
								<xsl:when test="COL19='SGD'">
									<xsl:value-of select="10"/>
								</xsl:when>
								<xsl:when test="COL19='MUL'">
									<xsl:value-of select="11"/>
								</xsl:when>
								<xsl:when test="COL19='ZAR'">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:when test="COL19='SEK'">
									<xsl:value-of select="13"/>
								</xsl:when>
								<xsl:when test="COL19='CNY'">
									<xsl:value-of select="15"/>
								</xsl:when>
								<xsl:when test="COL19='KRW'">
									<xsl:value-of select="16"/>
								</xsl:when>
								<xsl:when test="COL19='BDT'">
									<xsl:value-of select="17"/>
								</xsl:when>
								<xsl:when test="COL19='THB'">
									<xsl:value-of select="18"/>
								</xsl:when>
								<xsl:when test="COL19='VND'">
									<xsl:value-of select="19"/>
								</xsl:when>
								<xsl:when test="COL19='GBX'">
									<xsl:value-of select="20"/>
								</xsl:when>
								<xsl:when test="COL19='INR'">
									<xsl:value-of select="21"/>
								</xsl:when>
								<xsl:when test="COL19='CHF'">
									<xsl:value-of select="23"/>
								</xsl:when>
								<xsl:when test="COL19='CLP'">
									<xsl:value-of select="24"/>
								</xsl:when>
								<xsl:when test="COL19='COP'">
									<xsl:value-of select="25"/>
								</xsl:when>
								<xsl:when test="COL19='CZK'">
									<xsl:value-of select="26"/>
								</xsl:when>								
								<xsl:when test="COL19='DKK'">
									<xsl:value-of select="27"/>
								</xsl:when>
								<xsl:when test="COL19='GHS'">
									<xsl:value-of select="28"/>
								</xsl:when>
								<xsl:when test="COL19='HUF'">
									<xsl:value-of select="29"/>
								</xsl:when>
								<xsl:when test="COL19='IDR'">
									<xsl:value-of select="30"/>
								</xsl:when>
								<xsl:when test="COL19='ILS'">
									<xsl:value-of select="31"/>
								</xsl:when>
								<xsl:when test="COL19='ISK'">
									<xsl:value-of select="32"/>
								</xsl:when>
								<xsl:when test="COL19='KZT'">
									<xsl:value-of select="33"/>
								</xsl:when>
								<xsl:when test="COL19='LVL'">
									<xsl:value-of select="34"/>
								</xsl:when>
								<xsl:when test="COL19='MXN'">
									<xsl:value-of select="35"/>
								</xsl:when>
								<xsl:when test="COL19='NZD'">
									<xsl:value-of select="36"/>
								</xsl:when>
								<xsl:when test="COL19='PEN'">
									<xsl:value-of select="37"/>
								</xsl:when>
								<xsl:when test="COL19='PLN'">
									<xsl:value-of select="38"/>
								</xsl:when>
								<xsl:when test="COL19='RON'">
									<xsl:value-of select="40"/>
								</xsl:when>
								<xsl:when test="COL19='RUB'">
									<xsl:value-of select="41"/>
								</xsl:when>
								<xsl:when test="COL19='SKK'">
									<xsl:value-of select="42"/>
								</xsl:when>
								<xsl:when test="COL19='TRY'">
									<xsl:value-of select="43"/>
								</xsl:when>
								<xsl:when test="COL19='ARS'">
									<xsl:value-of select="44"/>
								</xsl:when>
								<xsl:when test="COL19='UYU'">
									<xsl:value-of select="45"/>
								</xsl:when>
								<xsl:when test="COL19='TWD'">
									<xsl:value-of select="46"/>
								</xsl:when>
								<xsl:when test="COL19='BMD'">
									<xsl:value-of select="47"/>
								</xsl:when>
								<xsl:when test="COL19='EEK'">
									<xsl:value-of select="48"/>
								</xsl:when>
								<xsl:when test="COL19='GEL'">
									<xsl:value-of select="49"/>
								</xsl:when>
								<xsl:when test="COL19='MYR'">
									<xsl:value-of select="51"/>
								</xsl:when>
								<xsl:when test="COL19='SIT'">
									<xsl:value-of select="52"/>
								</xsl:when>
								<xsl:when test="COL19='XAF'">
									<xsl:value-of select="53"/>
								</xsl:when>
								<xsl:when test="COL19='XOF'">
									<xsl:value-of select="54"/>
								</xsl:when>
								<xsl:when test="COL19='AZN'">
									<xsl:value-of select="55"/>
								</xsl:when>
								<xsl:when test="COL19='PKR'">
									<xsl:value-of select="56"/>
								</xsl:when>
								<xsl:when test="COL19='PHP'">
									<xsl:value-of select="57"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
											
						<CurrencyID>
							<xsl:value-of select="$varCurrencyI"/>
						</CurrencyID>
											
						
						<Multiplier>  
							<xsl:value-of select="translate(COL21,',','')"/>          
						</Multiplier>

						<AUECID>							
							<xsl:value-of select="16"/>
						</AUECID>
						
						<AssetID>
							<xsl:value-of select="3"/>
						</AssetID>
																	
						<ExchangeID>
							<xsl:value-of select="22"/>
						</ExchangeID>

						<UnderLyingID>
							<xsl:value-of select="1"/>
						</UnderLyingID>																						

						<xsl:variable name="varExpirationDate">
							<xsl:value-of select="COL20"/>
						</xsl:variable>
						
					    <ExpirationDate>
							<xsl:value-of select="$varExpirationDate"/>
						</ExpirationDate>
						
						<xsl:variable name="varDescription">
							<xsl:value-of select="COL3"/>
						</xsl:variable>
						
					    <LongName>
							<xsl:value-of select="$varDescription"/>
						</LongName>												
				
					</PositionMaster>

				</xsl:if>
			</xsl:for-each>
		</DocumentElement>

	</xsl:template>

	<xsl:variable name="lower_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="upper_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

</xsl:stylesheet>