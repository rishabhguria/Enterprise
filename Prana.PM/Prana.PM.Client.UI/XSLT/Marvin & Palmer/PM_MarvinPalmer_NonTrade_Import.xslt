<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
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
  <xsl:template name="FormatDate">
    <xsl:param name="varFullDate" />
    <xsl:variable name="varYear">
      <xsl:value-of select="substring($varFullDate, string-length($varFullDate) - 3 , 4)"/>
    </xsl:variable>
    <xsl:variable name="varWithoutYear">
      <xsl:value-of select="substring($varFullDate, 1, string-length($varFullDate) - 4)"/>
    </xsl:variable>
    <xsl:variable name="varDay">
      <xsl:value-of select="substring-after($varWithoutYear,'/')"/>
    </xsl:variable>
    <xsl:variable name="varMonth">
      <xsl:choose>
        <xsl:when test="$varWithoutYear &lt; 999">
          <xsl:value-of select="concat('0',substring($varWithoutYear, 1, 1))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="substring($varWithoutYear, 1, 2)"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:value-of select="concat($varMonth,$varDay,$varYear)"/>
  </xsl:template>

  <xsl:template match="/">
    <DocumentElement>
      <xsl:for-each select="//PositionMaster">

        <xsl:variable name="varCash">
          <xsl:call-template name="Translate">
            <xsl:with-param name="Number" select="normalize-space(COL132)"/>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="varDescription">
          <xsl:value-of select="normalize-space(COL9)" />
        </xsl:variable>
        <xsl:variable name="PB_SYMBOL_NAME">
          <xsl:value-of select="''"/>
        </xsl:variable>

        <xsl:if test="number($varCash)" >
          <PositionMaster>

            <xsl:variable name="PB_NAME">
              <xsl:value-of select="'NAV'" />
            </xsl:variable>
            
            <xsl:variable name="PB_FUND_NAME" select ="COL15"/>
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
			  
            <xsl:variable name = "PRANA_PRE_ACRONYM_NAME" >
              <xsl:choose>
			   <xsl:when test="contains($varDescription,'CAP') and $varCash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
                <xsl:when test="$varDescription='INT' and $varCash &gt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
                <xsl:when test="$varDescription='INT' and $varCash &lt; 0">
                  <xsl:value-of select="'Interest_Expense'"/>
                </xsl:when>
				  <xsl:when test="($varDescription= 'MGF' or $varDescription= 'CBF')  and $varCash &lt; 0">
					  <xsl:value-of select="'MGMTFEE'"/>
				  </xsl:when>
               
                <xsl:when test="contains($varDescription,'CAP') and $varCash &lt; 0">
                  <xsl:value-of select="'CashTransferOut'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>

            <xsl:variable name="PRANA_Post_ACRONYM_NAME">
              <xsl:choose>
			   <xsl:when test="contains($varDescription,'CAP') and $varCash &gt; 0">
                  <xsl:value-of select="'MISC_INC'"/>
                </xsl:when>
                <xsl:when test="$varDescription='INT' and $varCash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
                <xsl:when test="$varDescription='INT' and $varCash &gt; 0">
                  <xsl:value-of select="'Interest_Income'"/>
                </xsl:when>
				  <xsl:when test="($varDescription= 'MGF' or $varDescription= 'CBF')  and $varCash &lt; 0">
					  <xsl:value-of select="'Cash'"/>
				  </xsl:when>
				  
                <xsl:when test="contains($varDescription,'CAP') and $varCash &lt; 0">
                  <xsl:value-of select="'Cash'"/>
                </xsl:when>
               
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>


            <xsl:variable name="AbsCash">
              <xsl:choose>
                <xsl:when test="$varCash  &gt; 0">
                  <xsl:value-of select="$varCash"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="$varCash * (-1)"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
      
            <JournalEntries>
              <xsl:choose>
                <xsl:when test="$varCash &gt; 0">
                  <xsl:value-of select="concat('Cash', ':' , $AbsCash , '|' , $PRANA_Post_ACRONYM_NAME, ':' , $AbsCash)"/>
                </xsl:when>
                <xsl:when test="$varCash &lt; 0">
                  <xsl:value-of select="concat($PRANA_PRE_ACRONYM_NAME, ':' , $AbsCash , '|' , 'Cash', ':' , $AbsCash)"/>
                </xsl:when>                
              </xsl:choose>
            </JournalEntries>
           
            <Description>
              <xsl:choose>
				  <xsl:when test="$varDescription='CAP' and $varCash &gt; 0">
					  <xsl:value-of select="'Other Income'"/>
				  </xsl:when>
				   <xsl:when test="$varDescription='CAP' and $varCash &lt; 0">
					  <xsl:value-of select="'Cash Transfer'"/>
				  </xsl:when>
				  <xsl:when test="$varDescription='INT'  and $varCash &gt; 0">
					  <xsl:value-of select="'Interest Income'"/>
				  </xsl:when>
				  <xsl:when test="$varDescription='INT'  and $varCash &lt; 0">
					  <xsl:value-of select="'Interest Expense'"/>
				  </xsl:when>
				  <xsl:when test="($varDescription= 'MGF' or $varDescription= 'CBF')  and $varCash &lt; 0">
					  <xsl:value-of select="'Management Fees Expense'"/>
				  </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="''"/>
                </xsl:otherwise>
              </xsl:choose>
            </Description>         

            <CurrencyName>
              <xsl:value-of select="COL91"/>
            </CurrencyName>
            <xsl:variable name="varTradeDate">
              <xsl:value-of select="COL114"/>
            </xsl:variable>
            <Date>
              <xsl:value-of select="$varTradeDate"/>
            </Date>

          </PositionMaster>
        </xsl:if>
      </xsl:for-each>
    </DocumentElement>
  </xsl:template>
</xsl:stylesheet>